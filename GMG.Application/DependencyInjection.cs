using GMG.Application.Common.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GMG.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
        this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Registrar MediatR
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TenantContextBehaviour<,>));
            });

            return services;
        }
    }
}
