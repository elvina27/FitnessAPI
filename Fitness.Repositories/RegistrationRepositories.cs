using Fitness.Repositories.Anchors;
using Microsoft.Extensions.DependencyInjection;
using Fitness.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Repositories
{
    public static class RegistrationRepositories
    {
        public static void RegistrationRepository(this IServiceCollection service)
        {
            service.RegistrationOnInterface<IRepositoryAnchor>(ServiceLifetime.Scoped);
        }
    }
}
