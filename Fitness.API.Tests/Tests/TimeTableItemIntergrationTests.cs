using Fitness.API.Models.CreateRequest;
using Fitness.API.Models.Request;
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
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fitness.API.Tests.Tests
{
    public class TimeTableItemIntergrationTests : BaseIntegrationTest
    {
        private readonly Club club;
        private readonly Coach coach;
        private readonly Gym gym;
        private readonly Study study;

        public TimeTableItemIntergrationTests(FitnessApiFixture fixture) : base(fixture)
        {
            club = TestDataGenerator.Club();
            coach = TestDataGenerator.Coach();
            gym = TestDataGenerator.Gym();
            study = TestDataGenerator.Study();

            context.Clubs.Add(club);
            context.Coaches.Add(coach);
            context.Gyms.Add(gym);
            context.Studyes.Add(study);
            unitOfWork.SaveChangesAsync().Wait();
        }

        /// <summary>
        /// Тест на получение занятия по ID (GetById)
        /// </summary>
        [Fact]
        public async Task GetIdShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var timeTableItem1 = TestDataGenerator.TimeTableItem();
            var timeTableItem2 = TestDataGenerator.TimeTableItem();

            SetDependenciesOrTimeTableItem(timeTableItem1);
            SetDependenciesOrTimeTableItem(timeTableItem2);

            await context.TimeTableItems.AddRangeAsync(timeTableItem1, timeTableItem2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/TimeTableItem/{timeTableItem1.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var resultList = JsonConvert.DeserializeObject<List<TimeTableItemResponse>>(resultString);

            var result = resultList.FirstOrDefault();

            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    timeTableItem1.Id,
                    timeTableItem1.StartTime,
                    timeTableItem1.Warning
                });
        }

        /// <summary>
        /// Тест на получения всех занятий (GetAll)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var timeTableItem1 = TestDataGenerator.TimeTableItem();
            var timeTableItem2 = TestDataGenerator.TimeTableItem(x => x.DeletedAt = DateTimeOffset.Now);

            SetDependenciesOrTimeTableItem(timeTableItem1);
            SetDependenciesOrTimeTableItem(timeTableItem2);

            await context.TimeTableItems.AddRangeAsync(timeTableItem1, timeTableItem2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/TimeTableItem");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<TimeTableItemResponse>>(resultString);

            result.Should()
                .NotBeNull()
                .And
                .Contain(x => x.Id == timeTableItem1.Id)
                .And
                .NotContain(x => x.Id == timeTableItem2.Id);
        }

        /// <summary>
        /// Тест на добавление занятия (Add)
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var clientFactory = factory.CreateClient();

            var timeTableItem = mapper.Map<CreateTimeTableItemRequest>(TestDataGenerator.TimeTableItemRequestModel());
            timeTableItem.ClubId = club.Id;
            timeTableItem.CoachId = coach.Id;
            timeTableItem.GymId = gym.Id;
            timeTableItem.StudyId = study.Id;

            // Act
            string data = JsonConvert.SerializeObject(timeTableItem);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await clientFactory.PostAsync("/TimeTableItem", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TimeTableItemResponse>(resultString);

            var cinemaFirst = await context.TimeTableItems.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            cinemaFirst.Should()
                .BeEquivalentTo(timeTableItem);
        }
        
        /// <summary>
        /// Тест на изменение занятия по ID (Edit)
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var timeTableItem = TestDataGenerator.TimeTableItem();

            SetDependenciesOrTimeTableItem(timeTableItem);
            await context.TimeTableItems.AddAsync(timeTableItem);
            await unitOfWork.SaveChangesAsync();

            var timeTableItemRequest = mapper.Map<TimeTableItemRequest>(TestDataGenerator.TimeTableItemRequestModel(x => x.Id = timeTableItem.Id));
            SetDependenciesOrTimeTableItemRequestModelWithTimeTableItem(timeTableItem, timeTableItemRequest);

            // Act
            string data = JsonConvert.SerializeObject(timeTableItemRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/TimeTableItem", contextdata);

            var timeTableItemFirst = await context.TimeTableItems.FirstAsync(x => x.Id == timeTableItemRequest.Id);

            // Assert           
            timeTableItemFirst.Should()
                .BeEquivalentTo(timeTableItemRequest);
        }
        
        /// <summary>
        /// Тест на удаление занятия по ID (Delete)
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var timeTableItem = TestDataGenerator.TimeTableItem();

            SetDependenciesOrTimeTableItem(timeTableItem);
            await context.TimeTableItems.AddAsync(timeTableItem);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/TimeTableItem/{timeTableItem.Id}");

            var timeTableItemFirst = await context.TimeTableItems.FirstAsync(x => x.Id == timeTableItem.Id);

            // Assert
            timeTableItemFirst.DeletedAt.Should()
                .NotBeNull();

            timeTableItemFirst.Should()
                .BeEquivalentTo(new
                {
                    timeTableItem.StartTime,
                    timeTableItem.StudyId,
                    timeTableItem.CoachId,
                    timeTableItem.GymId,
                    timeTableItem.Warning,
                    timeTableItem.ClubId
                });
        }

        private void SetDependenciesOrTimeTableItem(TimeTableItem timeTableItem)
        {
            timeTableItem.ClubId = club.Id;
            timeTableItem.CoachId = coach.Id;
            timeTableItem.GymId = gym.Id;
            timeTableItem.StudyId = study.Id;
        }
        private void SetDependenciesOrTimeTableItemRequestModelWithTimeTableItem(TimeTableItem timeTableItem,TimeTableItemRequest timeTableItemRequest)
        {
            timeTableItemRequest.ClubId = timeTableItem.ClubId;
            timeTableItemRequest.CoachId = timeTableItem.CoachId;
            timeTableItemRequest.GymId = timeTableItem.GymId;
            timeTableItemRequest.StudyId = timeTableItem.StudyId;
        }
    }
}
