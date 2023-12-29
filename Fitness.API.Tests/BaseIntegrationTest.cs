using AutoMapper;
using Fitness.Api.AutoMappers;
using Fitness.API.Tests.Infrastructures;
using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts;
using Fitness.Services.AutoMappers;
using Xunit;

namespace Fitness.API.Tests
{
    /// <summary>
    /// Базовый класс для тестов
    /// </summary>
    [Collection(nameof(FitnessApiTestCollection))]
    public class BaseIntegrationTest
    {
        protected readonly CustomWebApplicationFactory factory;
        protected readonly IFitnessContext context;
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;

        public BaseIntegrationTest(FitnessApiFixture fixture)
        {
            factory = fixture.Factory;
            context = fixture.Context;
            unitOfWork = fixture.UnitOfWork;

            Profile[] profiles = { new APIMappers(), new ServiceMapper() };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(profiles);
            });

            mapper = config.CreateMapper();
        }
    }
}
