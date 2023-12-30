using Fitness.API.Models.CreateRequest;
using Fitness.API.Models.Response;
using Fitness.API.Tests.Infrastructures;
using Fitness.Context.Contracts.Models;
using Fitness.Tests.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fitness.API.Tests.Tests
{
    public class GymIntergrationTests : BaseIntegrationTest
    {
        public GymIntergrationTests(FitnessApiFixture fixture) : base(fixture)
        {
        }

        /// <summary>
        /// Тест на получение зала по ID (GetById)
        /// </summary>
        [Fact]
        public async Task GetIdShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var gym1 = TestDataGenerator.Gym();
            var gym2 = TestDataGenerator.Gym();

            await context.Gyms.AddRangeAsync(gym1, gym2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/Gym/{gym2.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GymResponse>(resultString);

            result.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    gym2.Id,
                    gym2.Title,
                    gym2.Capacity
                });
        }

        /// <summary>
        /// Тест на получения всех залов (GetAll)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var gym1 = TestDataGenerator.Gym();
            var gym2 = TestDataGenerator.Gym(x => x.DeletedAt = DateTimeOffset.Now);

            await context.Gyms.AddRangeAsync(gym1, gym2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/Gym");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<GymResponse>>(resultString);

            result.Should().NotBeNull()
                .And
                .Contain(x => x.Id == gym1.Id)
                .And
                .NotContain(x => x.Id == gym2.Id);
        }

        /// <summary>
        /// Тест на добавление зала (Add)
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var gym = mapper.Map<CreateGymRequest>(TestDataGenerator.GymModel());

            // Act
            string data = JsonConvert.SerializeObject(gym);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Gym", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GymResponse>(resultString);

            var placeFirst = await context.Gyms.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            placeFirst.Should()
                .BeEquivalentTo(gym);
        }

        /// <summary>
        /// Тест на изменение зала по ID (Edit)
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var gym = TestDataGenerator.Gym();
            await context.Gyms.AddAsync(gym);
            await unitOfWork.SaveChangesAsync();

            var gymRequest = TestDataGenerator.GymModel(x => x.Id = gym.Id);

            // Act
            string data = JsonConvert.SerializeObject(gymRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/Gym", contextdata);

            var gymFirst = await context.Gyms.FirstAsync(x => x.Id == gym.Id);

            // Assert           
            gymFirst.Should()
                .BeEquivalentTo(gymRequest);
        }

        /// <summary>
        /// Тест на удаление зала по ID (Delete)
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var gym = TestDataGenerator.Gym();
            await context.Gyms.AddAsync(gym);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/Gym/{gym.Id}");

            var gymFirst = await context.Gyms.FirstAsync(x => x.Id == gym.Id);

            // Assert
            gymFirst.DeletedAt.Should()
                .NotBeNull();

            gym.Should()
            .BeEquivalentTo(new
            {
                gym.Title,
                gym.Capacity
            });
        }
    }
}
