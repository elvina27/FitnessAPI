using Fitness.General;
using Fitness.Repositories.Anchors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Repositories
{
    /// <summary>
    /// Расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceExtensionsRepository
    {
        /// <summary>
        /// Регистрация репозиториев
        /// </summary>
        public static void RegistrationRepository(this IServiceCollection service)
        {
            service.RegistrationOnInterface<IRepositoryAnchor>(ServiceLifetime.Scoped);
        }
    }
}
