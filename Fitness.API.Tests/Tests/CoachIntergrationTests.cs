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
    public class CoachIntergrationTests : BaseIntegrationTest
    {
        public CoachIntergrationTests(FitnessApiFixture fixture) : base(fixture)
        {
        }

        /// <summary>
        /// Тест на получение тренера по ID (GetById)
        /// </summary>
        [Fact]
        public async Task GetIdShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var coach1 = TestDataGenerator.Coach();
            var coach2 = TestDataGenerator.Coach();

            await context.Coaches.AddRangeAsync(coach1, coach2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/Coach/{coach2.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CoachResponse>(resultString);

            result.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    coach2.Id,
                    coach2.Surname,
                    coach2.Name,
                    coach2.Patronymic,
                    coach2.Email,
                    coach2.Age
                });
        }

        /// <summary>
        /// Тест на получения всех тренеров (GetAll)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var coach1 = TestDataGenerator.Coach();
            var coach2 = TestDataGenerator.Coach(x => x.DeletedAt = DateTimeOffset.Now);

            await context.Coaches.AddRangeAsync(coach1, coach2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/Coach");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<CoachResponse>>(resultString);

            result.Should().NotBeNull()
                .And
                .Contain(x => x.Id == coach1.Id)
                .And
                .NotContain(x => x.Id == coach2.Id);
        }

        /// <summary>
        /// Тест на добавление тренера (Add)
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var coach = mapper.Map<CreateCoachRequest>(TestDataGenerator.CoachModel());

            // Act
            string data = JsonConvert.SerializeObject(coach);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Coach", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CoachResponse>(resultString);

            var placeFirst = await context.Coaches.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            placeFirst.Should()
                .BeEquivalentTo(coach);
        }

        /// <summary>
        /// Тест на изменение тренера по ID (Edit)
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var coach = TestDataGenerator.Coach();
            await context.Coaches.AddAsync(coach);
            await unitOfWork.SaveChangesAsync();

            var coachRequest = TestDataGenerator.CoachModel(x => x.Id = coach.Id);

            // Act
            string data = JsonConvert.SerializeObject(coachRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/Coach", contextdata);

            var coachFirst = await context.Coaches.FirstAsync(x => x.Id == coach.Id);

            // Assert           
            coachFirst.Should()
                .BeEquivalentTo(coachRequest);
        }

        /// <summary>
        /// Тест на удаление тренера по ID (Delete)
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var coach = TestDataGenerator.Coach();

            //coach.PersonId = person1.Id;

            await context.Coaches.AddAsync(coach);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/Coach/{coach.Id}");

            var coachFirst = await context.Coaches.FirstAsync(x => x.Id == coach.Id);

            // Assert
            coachFirst.DeletedAt.Should()
                .NotBeNull();

            coachFirst.Should()
                .BeEquivalentTo(new
                {
                    coach.Surname,
                    coach.Name,
                    coach.Patronymic,
                    coach.Email,
                    coach.Age,
                    coach.Documents
                });
        }
    }
}
