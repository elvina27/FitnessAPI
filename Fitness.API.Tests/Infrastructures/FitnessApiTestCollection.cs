using Xunit;

namespace Fitness.API.Tests.Infrastructures
{
    [CollectionDefinition(nameof(FitnessApiTestCollection))]
    public class FitnessApiTestCollection
        : ICollectionFixture<FitnessApiFixture>
    {
    }
}
