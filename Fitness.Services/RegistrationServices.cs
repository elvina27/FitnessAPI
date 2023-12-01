using Fitness.General;
using Fitness.Services.Anchors;
using Microsoft.Extensions.DependencyInjection;

namespace Fitness.Services
{
    public static class RegistrationServices
    {
        public static void RegistrationService(this IServiceCollection service)
        {
            service.RegistrationOnInterface<IServiceAnchor>(ServiceLifetime.Scoped);
        }
    }
}
