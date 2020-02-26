using Microsoft.Extensions.DependencyInjection;
using MindHorizon.Data.Contracts;
using MindHorizon.Data.UnitOfWork;
using MindHorizon.Services;
using MindHorizon.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindHorizon.IocConfig
{
    public static class AddCustomServicesExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }
    }
}
