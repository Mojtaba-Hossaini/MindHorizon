using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MindHorizon.Services;
using MindHorizon.Services.Contracts;
using MindHorizon.ViewModels.DynamicAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindHorizon.IocConfig
{
    public static class AddDynamicPersmissionExtentions
    {
        public static IServiceCollection AddDynamicPersmission(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, DynamicPermissionsAuthorizationHandler>();
            services.AddSingleton<IMvcActionsDiscoveryService, MvcActionsDiscoveryService>();
            services.AddSingleton<ISecurityTrimmingService, SecurityTrimmingService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ConstantPolicies.DynamicPermission, policy => policy.Requirements.Add(new DynamicPermissionRequirement()));
            });

            return services;
        }
    }
}
