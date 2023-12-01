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
    public static class RegistrationContext
    {
        public static void RegistrationContexts(this IServiceCollection service)
        {
            service.TryAddScoped<IFitnessContext>(provider => provider.GetRequiredService<FitnessContext>());
            service.TryAddScoped<IDbRead>(provider => provider.GetRequiredService<FitnessContext>());
            service.TryAddScoped<IDbWriter>(provider => provider.GetRequiredService<FitnessContext>());
            service.TryAddScoped<IUnitOfWork>(provider => provider.GetRequiredService<FitnessContext>());
        }
    }
}
