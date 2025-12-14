using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductionGrade.Services;
using ProductionGrade.Validation;

namespace ProductionGrade.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductionGradeServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<ProductService>();
            services.AddScoped<OrderService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<CartService>();

            // Validators
            services.AddValidatorsFromAssemblyContaining<ProductValidator>();
            services.AddValidatorsFromAssemblyContaining<OrderValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
            services.AddValidatorsFromAssemblyContaining<CartDtoValidator>();

            return services;
        }
    }
}
