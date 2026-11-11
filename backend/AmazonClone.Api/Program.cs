using Amazon.Infrastructure.Persistance;
using AmazonClone.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//--- GLOBAL REGISTRATION START ---

builder.Services.AddApplicationServices(builder.Configuration);

//--- GLOBAL REGISTRATION END ---


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(DependencyInjection.policyName);

app.MapControllers();

app.Run();


