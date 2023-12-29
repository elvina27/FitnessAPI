using AutoMapper;
using Fitness.Common.Entity.InterfaceDB;
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
    public class GymServiceTest : FitnessContextInMemory
    {
        private readonly IGymService gymService;
        private readonly GymReadRepository gymReadRepository;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="GymServiceTest"/>
        /// </summary>
        public GymServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ServiceMapper());
            });
            gymReadRepository = new GymReadRepository(Reader);

            gymService = new GymService(new GymWriteRepository(WriterContext), gymReadRepository, UnitOfWork,
                config.CreateMapper(), new ServicesValidatorService(new ClubReadRepository(Reader), new CoachReadRepository(Reader),
                gymReadRepository, new StudyReadRepository(Reader)));
        }

        /// <summary>
        /// Получение <see cref="Gym"/> по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => gymService.GetByIdAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Gym>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Получение <see cref="Gym"/> по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.Gym();
            await Context.Gyms.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await gymService.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(new
                {
                    target.Id,
                    target.Title,
                    target.Capacity
                });
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{Gym}"/> по идентификаторам возвращает пустую коллекцию
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await gymService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение <see cref="Gym"/> по идентификаторам возвращает данные
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnValues()
        {
            //Arrange
            var target = TestDataGenerator.Gym();

            await Context.Gyms.AddRangeAsync(target,
                TestDataGenerator.Gym(x => x.DeletedAt = DateTimeOffset.UtcNow));
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await gymService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == target.Id);
        }

        /// <summary>
        /// Удаление несуществуюущего <see cref="Gym"/>
        /// </summary>
        [Fact]
        public async Task DeletingNonExistentGymReturnExсeption()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => gymService.DeleteAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Gym>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Удаление удаленного <see cref="Gym"/>
        /// </summary>
        [Fact]
        public async Task DeletingDeletedGymReturnExсeption()
        {
            //Arrange
            var model = TestDataGenerator.Gym(x => x.DeletedAt = DateTime.UtcNow);
            await Context.Gyms.AddAsync(model);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            Func<Task> result = () => gymService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Gym>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Удаление <see cref="Gym"/>
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.Gym();
            await Context.Gyms.AddAsync(model);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => gymService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Gyms.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Добавление <see cref="Gym"/>
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.GymModel();

            //Act
            Func<Task> act = () => gymService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Gyms.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().BeNull();
        }

        /// <summary>
        /// Добавление не валидируемого <see cref="Gym"/>
        /// </summary>
        [Fact]
        public async Task AddShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.GymModel(x => x.Capacity = -1);

            //Act
            Func<Task> act = () => gymService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение несуществующего <see cref="Gym"/>
        /// </summary>
        [Fact]
        public async Task EditShouldNotFoundException()
        {
            //Arrange
            var model = TestDataGenerator.GymModel();

            //Act
            Func<Task> act = () => gymService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableEntityNotFoundException<Gym>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Изменение невалидируемого <see cref="Gym"/>
        /// </summary>
        [Fact]
        public async Task EditShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.GymModel(x => x.Capacity = -1);

            //Act
            Func<Task> act = () => gymService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение <see cref="Gym"/>
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.GymModel();
            var gym = TestDataGenerator.Gym(x => x.Id = model.Id);
            await Context.Gyms.AddAsync(gym);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => gymService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Gyms.Single(x => x.Id == gym.Id);
            entity.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    model.Id,
                    model.Title,
                    model.Capacity
                });
        }
    }
}

