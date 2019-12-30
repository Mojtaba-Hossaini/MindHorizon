using Microsoft.Extensions.DependencyInjection;
using MindHorizon.Data.Contracts;
using MindHorizon.Data.UnitOfWork;

namespace MindHorizon.IocConfig
{
    public static class AddCustomServicesExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
