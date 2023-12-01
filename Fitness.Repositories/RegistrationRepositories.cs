using Fitness.Repositories.Anchors;
using Microsoft.Extensions.DependencyInjection;
using Fitness.General;

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
