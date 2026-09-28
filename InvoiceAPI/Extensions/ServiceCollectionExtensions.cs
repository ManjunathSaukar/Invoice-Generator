using InvoiceAPI.Interfaces;
using InvoiceAPI.Repositories;
using InvoiceAPI.Services;

namespace InvoiceAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IDapperRepository, DapperRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            // Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInvoiceService, InvoiceService>();

            return services;
        }
    }
}
