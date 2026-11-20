using Amazon.Infrastructure.Persistance;
using Amazon.Infrastructure.Repositories;
using AmazonClone.Application.Interfaces;
using AmazonClone.Application.Services;
using AmazonClone.Application.Validators;
using AmazonClone.Infrastructure.External.Caching;
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

            //--- REDIS CONFIGURATION START ---

            string redisConnectionString = configuration.GetConnectionString("Redis") 
                ?? throw new InvalidOperationException("Connection string 'Redis' not proided");

            services.AddStackExchangeRedisCache(options => 
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "amazon:clone";
            });

            //--- REDIS CONFIGURATION END ---


            // --- DI REGISTRATION START ---

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            // Because : Reuses one thread-safe Redis connection across all requests to avoid TCP connection overhead.
            services.AddSingleton<ICacheService, CacheService>();
            // --- DI REGISTRATION END ---


            // --- FLUEND VALIDATION REG START ---

            services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

            // --- FLUEND VALIDATION REG END ---

            return services;
        }
    }
}
