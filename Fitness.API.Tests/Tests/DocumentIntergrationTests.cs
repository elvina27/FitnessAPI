using Fitnes.API.Models.CreateRequest;
using Fitnes.API.Models.Response;
using Fitness.API.Models.Request;
using Fitness.API.Tests.Infrastructures;
using Fitness.Context.Contracts.Models;
using Fitness.Tests.Extensions;
using FluentAssertions;
using FluentAssertions.Execution;
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
    public class DocumentIntergrationTests : BaseIntegrationTest
    {
        private readonly Coach coach1;
        private readonly Coach coach2;
        public DocumentIntergrationTests(FitnessApiFixture fixture) : base(fixture)
        {
            coach1 = TestDataGenerator.Coach();
            coach2 = TestDataGenerator.Coach();
            context.Coaches.AddRangeAsync(coach1, coach2);
            unitOfWork.SaveChangesAsync().Wait();
        }

        /// <summary>
        /// Тест на получение работника по ID (GetById)
        /// </summary>
        [Fact]
        public async Task GetIdShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var document1 = TestDataGenerator.Document();
            var document2 = TestDataGenerator.Document();

            document1.CoachId = coach1.Id;
            document2.CoachId = coach2.Id;

            await context.Documents.AddRangeAsync(document1, document2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/Document/{document2.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DocumentResponse>(resultString);

            result.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    document2.Id,
                    document2.Number,
                    document2.Series,
                    document2.IssuedAt
                });
        }

        /// <summary>
        /// Тест на получения всех работников (GetAll)
        /// </summary>
        [Fact]
        public async Task GetAllShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var document1 = TestDataGenerator.Document();
            var document2 = TestDataGenerator.Document(x => x.DeletedAt = DateTimeOffset.Now);

            document1.CoachId = coach1.Id;
            document2.CoachId = coach2.Id;

            await context.Documents.AddRangeAsync(document1, document2);
            await unitOfWork.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/Document");

            // Assert
            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<DocumentResponse>>(resultString);

            result.Should().NotBeNull()
                .And
                .Contain(x => x.Id == document1.Id)
                .And
                .NotContain(x => x.Id == document2.Id);
        }

        /*/// <summary>
        /// Тест на добавление работника (Add)
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var document = mapper.Map<CreateDocumentRequest>(TestDataGenerator.DocumentRequestModel());

            document.Coach = coach1.Id;

            // Act
            string data = JsonConvert.SerializeObject(document);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Document", contextdata);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DocumentResponse>(resultString);

            var documentFirst = await context.Documents.FirstAsync(x => x.Id == result!.Id);

            // Assert          
            documentFirst.CoachId.Should().Be(document.Coach);
        }

        /// <summary>
        /// Тест на изменение работника по ID (Edit)
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var document = TestDataGenerator.Document();
            document.CoachId = coach1.Id;
            await context.Documents.AddAsync(document);
            await unitOfWork.SaveChangesAsync();

            var documentRequest = TestDataGenerator.DocumentRequest(x => x.Id = document.Id);

            // Act
            string data = JsonConvert.SerializeObject(documentRequest);
            var contextdata = new StringContent(data, Encoding.UTF8, "application/json");
            await client.PutAsync("/Document", contextdata);

            var documentFirst = await context.Documents.FirstAsync(x => x.Id == document.Id);

            // Assert           
            documentFirst.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    document.Id,
                    document.CoachId,
                    document.Experience,
                    document.Number,
                    document.Email
                });
        }

        /// <summary>
        /// Тест на удаление работника по ID (Delete)
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            // Arrange
            var client = factory.CreateClient();
            var document = TestDataGenerator.Document();

            document.CoachId = coach1.Id;

            await context.Documents.AddAsync(document);
            await unitOfWork.SaveChangesAsync();

            // Act
            await client.DeleteAsync($"/Document/{document.Id}");

            var documentFirst = await context.Documents.FirstAsync(x => x.Id == document.Id);

            // Assert
            documentFirst.DeletedAt.Should()
                .NotBeNull();

            documentFirst.Should()
                .BeEquivalentTo(new
                {
                    document.Number,
                    document.Series,
                    document.IssuedAt,
                    document.DocumentType
                });
        }*/
    }
}

