using AmazonClone.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace AmazonClone.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(
            RequestDelegate next, 
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment env
            )
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, context);
            }
        }

        private async Task HandleExceptionAsync(Exception ex, HttpContext context)
        {
            // Avoid modifying the response if headers were already sent to the client
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started; the exception middleware cannot modify headers.");
                return;
            }
            int statusCode;
            string detail;
            string title;

            Dictionary<string, List<string>> ? validationErrors = null;

            if (ex is DomainException) 
            {
                statusCode = StatusCodes.Status400BadRequest;
                title = "Business rule validation";
                detail = ex.Message;
            }
            else if (ex is ValidationException validationException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                title = "Validation failed";
                detail = "One or more validation errors occurred.";
                validationErrors = new Dictionary<string, List<string>>();
                foreach (FluentValidation.Results.ValidationFailure failure in validationException.Errors)
                {
                    if (!validationErrors.ContainsKey(failure.PropertyName))
                    {
                        validationErrors[failure.PropertyName] = new List<string>();
                    }
                    validationErrors[failure.PropertyName].Add(failure.ErrorMessage);
                }
            }
            else if (ex is BrokenCircuitException)
            {
                statusCode = StatusCodes.Status503ServiceUnavailable;
                title = "Service temporarily unavailable";
                detail = "A dependent service is failing. Please try again shortly.";
            }
            else if (ex is TimeoutRejectedException || ex is TimeoutException)
            {
                statusCode = StatusCodes.Status504GatewayTimeout;
                title = "Dependent service timed out";
                detail = "A dependent service took too long to respond.";
            }
            else if (ex is HttpRequestException)
            {
                statusCode = StatusCodes.Status502BadGateway;
                title = "Dependent service error";
                detail = "A dependent service returned an error.";
            }
            else if (ex is DbUpdateException)
            {
                statusCode = StatusCodes.Status409Conflict;
                title = "Data conflict";
                detail = "The change conflicts with existing data.";
            }
            else
            {
                statusCode = StatusCodes.Status500InternalServerError;
                title = "Unexpected error";
                detail = _env.IsDevelopment()
                    ? ex.Message
                    : "An unexpected error occurred. Please contact support.";
            }

            // Log 5xx as Error, 4xx as Warning to keep error logs actionable
            if (statusCode >= 500)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing {Path}", context.Request.Path);
            }
            else
            {
                _logger.LogWarning(ex, "A client error ({StatusCode}) occurred while processing {Path}", statusCode, context.Request.Path);
            }

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            
            problem.Extensions["correlationId"] = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;


            if (validationErrors is not null)
            {
                problem.Extensions["errors"] = validationErrors;
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(
                problem,
                options:null,
                contentType:"application/problem+json"
            );
        }
    }
}
