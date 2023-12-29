using AutoMapper;
using Fitness.Services.AutoMappers;
using Xunit;

namespace Fitness.Services.Tests.TestsServices
{
    public class MapperTest
    {
        [Fact]
        public void TestMapper()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ServiceMapper>());
            configuration.AssertConfigurationIsValid();
        }
    }
}
