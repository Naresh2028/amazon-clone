using Amazon.Infrastructure.Persistance;
using Amazon.Infrastructure.Repositories;
using AmazonClone.Application.Interfaces;
using AmazonClone.Application.Services;
using AmazonClone.Application.Validators;
using AmazonClone.Infrastructure.Persistance;
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

            string[] allowedOrigins = configuration.GetSection("cors:AllowedOrigins").Get<string[]>()
                ?? throw new InvalidOperationException("CORS origin URL was not configured");

            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy(POLICY_NAME, options =>
                {
                    options
                        .WithOrigins(allowedOrigins)
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // --- DI REGISTRATION END ---


            // --- FLUEND VALIDATION REG START ---

            services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

            // --- FLUEND VALIDATION REG END ---

            return services;
        }
    }
}
