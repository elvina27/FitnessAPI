using Fitness.API.Models.CreateRequest;
using Fitness.API.Models.Response;
using Fitness.API.Tests.Infrastructures;
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
    public class StudyIntergrationTests : BaseIntegrationTest
    {
        public StudyIntergrationTests(FitnessApiFixture fixture) : base(fixture)
        {
        }

        /// <summary>
        /// Тест на получение занятия по ID (GetById)
        /// </summary>
        [Fact]
        public async Task GetIdShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var study1 = TestDataGenerator.Study();
            var study2 = TestDataGenerator.Study();

            await context.Studyes.AddRangeAsync(study1, study2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/Study/{study2.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StudyResponse>(resultString);

            result.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    study2.Id,
                    study2.Title,
                    study2.Description,
                    study2.Duration
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
            var study1 = TestDataGenerator.Study();
            var study2 = TestDataGenerator.Study(x => x.DeletedAt = DateTimeOffset.Now);

            await context.Studyes.AddRangeAsync(study1, study2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/Study");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<StudyResponse>>(resultString);

            result.Should().NotBeNull()
                .And
                .Contain(x => x.Id == study1.Id)
                .And
                .NotContain(x => x.Id == study2.Id);
        }

        /// <summary>
        /// Тест на добавление занятия (Add)
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var study = mapper.Map<CreateStudyRequest>(TestDataGenerator.StudyModel());

            // Act
            string data = JsonConvert.SerializeObject(study);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Study", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StudyResponse>(resultString);

            var placeFirst = await context.Studyes.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            placeFirst.Should()
                .BeEquivalentTo(study);
        }

        /// <summary>
        /// Тест на изменение занятия по ID (Edit)
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var study = TestDataGenerator.Study();
            await context.Studyes.AddAsync(study);
            await unitOfWork.SaveChangesAsync();

            var studyRequest = TestDataGenerator.StudyModel(x => x.Id = study.Id);

            // Act
            string data = JsonConvert.SerializeObject(studyRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/Study", contextdata);

            var studyFirst = await context.Studyes.FirstAsync(x => x.Id == study.Id);

            // Assert           
            studyFirst.Should()
                .BeEquivalentTo(studyRequest);
        }

        /// <summary>
        /// Тест на удаление занятия по ID (Delete)
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var study = TestDataGenerator.Study();
            await context.Studyes.AddAsync(study);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/Study/{study.Id}");

            var studyFirst = await context.Studyes.FirstAsync(x => x.Id == study.Id);

            // Assert
            studyFirst.DeletedAt.Should()
                .NotBeNull();

            study.Should()
            .BeEquivalentTo(new
            {
                study.Title,
                study.Description,
                study.Duration,
                study.Category
            });
        }
    }
}
