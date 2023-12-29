using AutoMapper;
using Fitness.Context.Contracts.Models;
using Fitness.Context.Tests;
using Fitness.Repositories.ReadRepositories;
using Fitness.Repositories.WriteRepositories;
using Fitness.Services.AutoMappers;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.ServicesContracts;
using Fitness.Services.Implementations;
using Fitness.Services.Validator;
using Fitness.Tests.Extensions;
using FluentAssertions;
using Xunit;

namespace Fitness.Services.Tests.TestsServices
{
    public class DocumentServiceTest : FitnessContextInMemory
    {
        private readonly IDocumentService documentService;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="DocumentServiceTest"/>
        /// </summary>
        public DocumentServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ServiceMapper());
            });

            documentService = new DocumentService(
                new DocumentReadRepository(Reader),
                new DocumentWriteRepository(WriterContext),
                // UnitOfWork,
                config.CreateMapper(),
                new CoachReadRepository(Reader),              
                new ServicesValidatorService(
                    new ClubReadRepository(Reader),
                    new CoachReadRepository(Reader),                    
                    new GymReadRepository(Reader), 
                    new StudyReadRepository(Reader)));
        }

        /// <summary>
        /// Получение <see cref="Document"/> по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => documentService.GetByIdAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Document>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Получение <see cref="Document"/> по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.Document();
            await Context.Documents.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await documentService.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(new
                {
                    target.Id,
                    target.DocumentType,
                   // target.Number,
                   // target.Series,
                    target.IssuedAt,
                    target.IssuedBy //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! нужно ли DocumentType?
                });
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{Document}"/> по идентификаторам возвращает пустйю коллекцию
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await documentService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{Document}"/> по идентификаторам возвращает данные
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnValues()
        {
            //Arrange
            var target = TestDataGenerator.Document();
            await Context.Documents.AddRangeAsync(target,
                TestDataGenerator.Document(x => x.DeletedAt = DateTimeOffset.UtcNow));
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await documentService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(0);
        }

        /// <summary>
        /// Удаление не существуюущего <see cref="Document"/>
        /// </summary>
        [Fact]
        public async Task DeletingNonExistentDocumentReturnExсeption()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => documentService.DeleteAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Document>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Удаление удаленного <see cref="Document"/>
        /// </summary>
        [Fact]
        public async Task DeletingDeletedDocumentReturnExсeption()
        {
            //Arrange
            var model = TestDataGenerator.Document(x => x.DeletedAt = DateTime.UtcNow);
            await Context.Documents.AddAsync(model);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            Func<Task> result = () => documentService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Document>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Удаление <see cref="Document"/>
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.Document();
            await Context.Documents.AddAsync(model);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => documentService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Documents.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Добавление <see cref="Document"/>
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            //Arrange
            var coach = TestDataGenerator.Coach();

            await Context.Coaches.AddAsync(coach);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            var model = TestDataGenerator.DocumentRequestModel();
            model.CoachId = coach.Id;


            //Act
            Func<Task> act = () => documentService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Documents.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().BeNull();
        }

        /// <summary>
        /// Добавление не валидируемого <see cref="Document"/>
        /// </summary>
        [Fact]
        public async Task AddShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.DocumentRequestModel();

            //Act
            Func<Task> act = () => documentService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение несуществующего <see cref="Document"/>
        /// </summary>
        [Fact]
        public async Task EditShouldNotFoundException()
        {
            //Arrange
            var coach = TestDataGenerator.Coach();

            await Context.Coaches.AddAsync(coach);

            await UnitOfWork.SaveChangesAsync(CancellationToken);

            var model = TestDataGenerator.DocumentRequestModel();
            model.CoachId = coach.Id;

            //Act
            Func<Task> act = () => documentService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableEntityNotFoundException<Document>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Изменение невалидируемого <see cref="Document"/>
        /// </summary>
        [Fact]
        public async Task EditShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.DocumentRequestModel();

            //Act
            Func<Task> act = () => documentService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение <see cref="Document"/>
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            //Arrange
            var coach = TestDataGenerator.Coach();

            await Context.Coaches.AddAsync(coach);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            var document = TestDataGenerator.Document();
            document.CoachId = coach.Id;

            var model = TestDataGenerator.DocumentRequestModel();
            model.Id = document.Id;
            model.CoachId = coach.Id;

            await Context.Documents.AddAsync(document);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => documentService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Documents.Single(x => x.Id == document.Id);
            entity.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    model.Id,
                    model.DocumentType,
                    // target.Number,
                    // target.Series,
                    model.IssuedAt,
                    model.IssuedBy
                });
        }
    }
}
