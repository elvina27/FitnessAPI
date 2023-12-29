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
    public class CoachServiceTest : FitnessContextInMemory
    {
        private readonly ICoachService coachService;
        private readonly CoachReadRepository coachReadRepository;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="CoachServiceTest"/>
        /// </summary>
        public CoachServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ServiceMapper());
            });

            coachReadRepository = new CoachReadRepository(Reader);

            coachService = new CoachService(new CoachWriteRepository(WriterContext), coachReadRepository, UnitOfWork, 
                config.CreateMapper(), new ServicesValidatorService(new ClubReadRepository(Reader), coachReadRepository,
                new GymReadRepository(Reader), new StudyReadRepository(Reader))); 
        }

        /// <summary>
        /// Получение <see cref="Coach"/> по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => coachService.GetByIdAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Coach>>()
               .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Получение <see cref="Coach"/> по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.Coach();
            await Context.Coaches.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await coachService.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(new
                {
                    target.Id,
                    target.Surname,
                    target.Name,
                    target.Patronymic,
                    target.Email,
                    target.Age
                });
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{Coach}"/> по идентификаторам возвращает пустую коллекцию
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await coachService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{Coach}"/> по идентификаторам возвращает данные
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnValues()
        {
            //Arrange
            var target = TestDataGenerator.Coach();

            await Context.Coaches.AddRangeAsync(target,
                TestDataGenerator.Coach(x => x.DeletedAt = DateTimeOffset.UtcNow));
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await coachService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == target.Id);
        }

        /// <summary>
        /// Удаление несуществуюущего <see cref="Coach"/>
        /// </summary>
        [Fact]
        public async Task DeletingNonExistentCoachReturnExсeption()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => coachService.DeleteAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Coach>>()
               .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Удаление удаленного <see cref="Coach"/>
        /// </summary>
        [Fact]
        public async Task DeletingDeletedCoachReturnExсeption()
        {
            //Arrange
            var model = TestDataGenerator.Coach(x => x.DeletedAt = DateTime.UtcNow);
            await Context.Coaches.AddAsync(model);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            Func<Task> result = () => coachService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Coach>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Удаление <see cref="Coach"/>
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.Coach();
            await Context.Coaches.AddAsync(model);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => coachService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Coaches.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Добавление <see cref="Coach"/>
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.CoachModel();

            //Act
            Func<Task> act = () => coachService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Coaches.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().BeNull();
        }

        /// <summary>
        /// Добавление невалидируемого <see cref="Coach"/>
        /// </summary>
        [Fact]
        public async Task AddShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.CoachModel(x => x.Name = "T");

            //Act
            Func<Task> act = () => coachService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение несуществующего <see cref="Coach"/>
        /// </summary>
        [Fact]
        public async Task EditShouldNotFoundException()
        {
            //Arrange
            var model = TestDataGenerator.CoachModel();

            //Act
            Func<Task> act = () => coachService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableEntityNotFoundException<Coach>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Изменение невалидируемого <see cref="Coach"/>
        /// </summary>
        [Fact]
        public async Task EditShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.CoachModel(x => x.Name = "T");

            //Act
            Func<Task> act = () => coachService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение <see cref="Coach"/>
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.CoachModel();
            var coach = TestDataGenerator.Coach(x => x.Id = model.Id);
            await Context.Coaches.AddAsync(coach);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => coachService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Coaches.Single(x => x.Id == coach.Id);
            entity.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    model.Id,
                    model.Surname,
                    model.Name,
                    model.Patronymic,
                    model.Email,
                    model.Age
                });
        }
    }
}
