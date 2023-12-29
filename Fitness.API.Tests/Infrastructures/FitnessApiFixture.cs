using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context;
using Fitness.Context.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fitness.API.Tests.Infrastructures
{
    public class FitnessApiFixture : IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory factory;
        private FitnessContext? fitnessContext;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="FitnessApiFixture"/>
        /// </summary>
        public FitnessApiFixture()
        {
            factory = new CustomWebApplicationFactory();
        }

        Task IAsyncLifetime.InitializeAsync() => FitnessContext.Database.MigrateAsync();

        async Task IAsyncLifetime.DisposeAsync()
        {
            await FitnessContext.Database.EnsureDeletedAsync();
            await FitnessContext.Database.CloseConnectionAsync();
            await FitnessContext.DisposeAsync();
            await factory.DisposeAsync();
        }

        public CustomWebApplicationFactory Factory => factory;

        public IFitnessContext Context => FitnessContext;

        public IUnitOfWork UnitOfWork => FitnessContext;

        internal FitnessContext FitnessContext
        {
            get
            {
                if (fitnessContext != null)
                {
                    return fitnessContext;
                }

                var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
                fitnessContext = scope.ServiceProvider.GetRequiredService<FitnessContext>();
                return fitnessContext;
            }
        }
    }
}
