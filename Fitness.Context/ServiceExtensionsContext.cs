using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Context
{
    /// <summary>
    /// Методы пасширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceExtensionsContext
    {
        /// <summary>
        /// Регистрирует все что связано с контекстом
        /// </summary>
        /// <param name="service"></param>
        public static void RegistrationContext(this IServiceCollection service)
        {
            service.TryAddScoped<IFitnessContext>(provider => provider.GetRequiredService<FitnessContext>());
            service.TryAddScoped<IDbRead>(provider => provider.GetRequiredService<FitnessContext>());
            service.TryAddScoped<IDbWriter>(provider => provider.GetRequiredService<FitnessContext>());
            service.TryAddScoped<IUnitOfWork>(provider => provider.GetRequiredService<FitnessContext>());
        }
    }
}
