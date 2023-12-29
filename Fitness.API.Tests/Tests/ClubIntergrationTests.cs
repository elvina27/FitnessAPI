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

        /// <summary>
        /// Тест на получение клуба по ID (GetById)
        /// </summary>
        [Fact]
        public async Task GetIdShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var сlub1 = TestDataGenerator.Club();
            var сlub2 = TestDataGenerator.Club();

            await context.Clubs.AddRangeAsync(сlub1, сlub2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/Club/{сlub2.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ClubResponse>(resultString);

            result.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    сlub2.Id,
                    сlub2.Title,
                    сlub2.Metro,
                    сlub2.Address,
                    сlub2.Email
                });
        }

        /// <summary>
        /// Тест на получения всех клубов (GetAll)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var сlub1 = TestDataGenerator.Club();
            var сlub2 = TestDataGenerator.Club(x => x.DeletedAt = DateTimeOffset.Now);

            await context.Clubs.AddRangeAsync(сlub1, сlub2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/Club");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ClubResponse>>(resultString);

            result.Should().NotBeNull()
                .And
                .Contain(x => x.Id == сlub1.Id)
                .And
                .NotContain(x => x.Id == сlub2.Id);
        }

        /// <summary>
        /// Тест на добавление клуба (Add)
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var сlub = TestDataGenerator.Club();

            // Act
            string data = JsonConvert.SerializeObject(сlub);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Club", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ClubResponse>(resultString);

            var placeFirst = await context.Clubs.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            placeFirst.Should()
                .BeEquivalentTo(сlub);
        }

        /// <summary>
        /// Тест на изменение клуба по ID (Edit)
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var сlub = TestDataGenerator.Club();
            await context.Clubs.AddAsync(сlub);
            await unitOfWork.SaveChangesAsync();

            var сlubRequest = TestDataGenerator.ClubModel(x => x.Id = сlub.Id);

            // Act
            string data = JsonConvert.SerializeObject(сlubRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/Club", contextdata);

            var сlubFirst = await context.Clubs.FirstAsync(x => x.Id == сlub.Id);

            // Assert           
            сlubFirst.Should()
                .BeEquivalentTo(сlubRequest);
        }

        /// <summary>
        /// Тест на удаление клуба по ID (Delete)
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var сlub = TestDataGenerator.Club();
            await context.Clubs.AddAsync(сlub);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/Club/{сlub.Id}");

            var сlubFirst = await context.Clubs.FirstAsync(x => x.Id == сlub.Id);

            // Assert
            сlubFirst.DeletedAt.Should()
                .NotBeNull();

            сlub.Should()
            .BeEquivalentTo(new
                {
                    сlub.Title,
                    сlub.Metro,
                    сlub.Address,
                    сlub.Email
                });
        }
    }
}