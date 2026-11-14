using Amazon.Infrastructure.Persistance;
using Amazon.Infrastructure.Repositories;
using AmazonClone.Application.Interfaces;
using AmazonClone.Application.Services;
using AmazonClone.Application.Validators;
using AmazonClone.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AmazonClone.Api.Extensions
{
    public static class DependencyInjection
    {

        public const string POLICY_NAME = "amazon-client";
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) 
        {


            // --- CONNECTION STRING START ---
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not provided");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            // --- CONNECTION STRING END ---

            //--- CORS CONFIGURATION STARTS ---

            var corsUrl = configuration.GetValue<string>("cors")
                ?? throw new InvalidOperationException("CORS origin URL was not configured");

            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy(POLICY_NAME, options =>
                {
                    options
                        .WithOrigins(corsUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            //--- CORS CONFIGURATION END ---


            // --- DI REGISTRATION START ---

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            // --- DI REGISTRATION END ---


            // --- FLUEND VALIDATION REG START ---

            services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

            // --- FLUEND VALIDATION REG END ---

            return services;
        }
    }
}
