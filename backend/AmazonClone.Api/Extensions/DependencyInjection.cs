using Amazon.Infrastructure.Persistance;
using Amazon.Infrastructure.Repositories;
using AmazonClone.Application.Interfaces;
using AmazonClone.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace AmazonClone.Api.Extensions
{
    public static class DependencyInjection
    {

        public const string policyName = "amazon-client";
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) 
        {


            // --- CONNECTION STRING START ---
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not provided, da");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            // --- CONNECTION STRING END ---

            //--- CORS CONFIGURATION STARTS ---

            var corsUrl = configuration.GetValue<string>("cors")
                ?? throw new InvalidOperationException("CORS origin URL was not configured, da!");

            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy(policyName, options =>
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

            // --- DI REGISTRATION END ---

            return services;
        }
    }
}
