using Fitness.API.Models.CreateRequest;
using Fitness.API.Models.Request;
using Fitness.API.Models.Response;
using Fitness.API.Tests.Infrastructures;
using Fitness.Tests.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Xunit;

namespace Fitness.API.Tests.Tests
{
    public class ClubIntergrationTests : BaseIntegrationTest
    {
        public ClubIntergrationTests(FitnessApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var club = mapper.Map<CreateClubRequest>(TestDataGenerator.ClubModel());

            // Act
            string data = JsonConvert.SerializeObject(club);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Club", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ClubResponse>(resultString);

            var clubFirst = await context.Clubs.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            clubFirst.Should()
                .BeEquivalentTo(club);
        }

        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var club = TestDataGenerator.Club();
            await context.Clubs.AddAsync(club);
            await unitOfWork.SaveChangesAsync();

            var clubRequest = mapper.Map<ClubRequest>(TestDataGenerator.ClubModel(x => x.Id = club.Id));

            // Act
            string data = JsonConvert.SerializeObject(clubRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/Club", contextdata);

            var clubFirst = await context.Clubs.FirstAsync(x => x.Id == clubRequest.Id);

            // Assert           
            clubFirst.Should()
                .BeEquivalentTo(clubRequest);
        }

        [Fact]
        public async Task GetShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var club1 = TestDataGenerator.Club();
            var club2 = TestDataGenerator.Club();

            await context.Clubs.AddRangeAsync(club1, club2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/Club/{club1.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ClubResponse>(resultString);

            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    club1.Id,
                    club1.Title,
                    club1.Address
                });
        }

        [Fact]
        public async Task GetAllShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var club1 = TestDataGenerator.Club();
            var club2 = TestDataGenerator.Club(x => x.DeletedAt = DateTimeOffset.Now);

            await context.Clubs.AddRangeAsync(club1, club2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/Club");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ClubResponse>>(resultString);

            result.Should()
                .NotBeNull()
                .And
                .Contain(x => x.Id == club1.Id)
                .And
                .NotContain(x => x.Id == club2.Id);
        }

        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var club = TestDataGenerator.Club();
            await context.Clubs.AddAsync(club);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/Club/{club.Id}");

            var clubFirst = await context.Clubs.FirstAsync(x => x.Id == club.Id);

            // Assert
            clubFirst.DeletedAt.Should()
                .NotBeNull();

            clubFirst.Should()
                .BeEquivalentTo(new
                {
                    club.Title,
                    club.Address
                });
        }
    }
}

