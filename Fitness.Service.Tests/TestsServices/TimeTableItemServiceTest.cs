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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fitness.Services.Tests.TestsServices
{
    public class TimeTableItemServiceTest : FitnessContextInMemory
    {
        private readonly ITimeTableItemService timeTableItemService;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="TimeTableItemServiceTest"/>
        /// </summary>
        public TimeTableItemServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ServiceMapper());
            });

            timeTableItemService = new TimeTableItemService(
                new TimeTableItemWriteRepository(WriterContext), new TimeTableItemReadRepository(Reader),
                new ClubReadRepository(Reader), new CoachReadRepository(Reader), new DocumentReadRepository(Reader),
                new GymReadRepository(Reader), new StudyReadRepository(Reader), config.CreateMapper(), UnitOfWork,
                new ServicesValidatorService(new ClubReadRepository(Reader), new CoachReadRepository(Reader),                    
                new GymReadRepository(Reader), new StudyReadRepository(Reader)));
        }

        /// <summary>
        /// Получение <see cref="TimeTableItem"/> по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => timeTableItemService.GetByIdAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<TimeTableItem>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Получение <see cref="TimeTableItem"/> по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.TimeTableItem();
            await Context.TimeTableItems.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await timeTableItemService.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(new
                {
                    target.Id,
                    target.StartTime,
                    target.Warning
                });
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{TimeTableItem}"/> по идентификаторам возвращает пустйю коллекцию
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await timeTableItemService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{TimeTableItem}"/> по идентификаторам возвращает данные
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnValues()
        {
            //Arrange
            var target = TestDataGenerator.TimeTableItem();
            await Context.TimeTableItems.AddRangeAsync(target,
                TestDataGenerator.TimeTableItem(x => x.DeletedAt = DateTimeOffset.UtcNow));
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await timeTableItemService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(0);
        }

        /// <summary>
        /// Удаление не существуюущего <see cref="TimeTableItem"/>
        /// </summary>
        [Fact]
        public async Task DeletingNonExistentTimeTableItemReturnExсeption()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => timeTableItemService.DeleteAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<TimeTableItem>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Удаление удаленного <see cref="TimeTableItem"/>
        /// </summary>
        [Fact]
        public async Task DeletingDeletedTimeTableItemReturnExсeption()
        {
            //Arrange
            var model = TestDataGenerator.TimeTableItem(x => x.DeletedAt = DateTime.UtcNow);
            await Context.TimeTableItems.AddAsync(model);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            Func<Task> result = () => timeTableItemService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<TimeTableItem>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Удаление <see cref="TimeTableItem"/>
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.TimeTableItem();
            await Context.TimeTableItems.AddAsync(model);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => timeTableItemService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.TimeTableItems.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Добавление <see cref="TimeTableItem"/>
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            //Arrange
            var club = TestDataGenerator.Club();
            var coach = TestDataGenerator.Coach();
            var gym = TestDataGenerator.Gym();
            var study = TestDataGenerator.Study();

            await Context.Clubs.AddAsync(club);
            await Context.Coaches.AddAsync(coach);
            await Context.Gyms.AddAsync(gym);
            await Context.Studyes.AddAsync(study);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            var model = TestDataGenerator.TimeTableItemRequestModel();
            model.ClubId = club.Id;
            model.CoachId = coach.Id;
            model.GymId = gym.Id;
            model.StudyId = study.Id;

            //Act
            Func<Task> act = () => timeTableItemService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.TimeTableItems.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().BeNull();
        }

        /// <summary>
        /// Добавление не валидируемого <see cref="TimeTableItem"/>
        /// </summary>
        [Fact]
        public async Task AddShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.TimeTableItemRequestModel();

            //Act
            Func<Task> act = () => timeTableItemService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение несуществующего <see cref="TimeTableItem"/>
        /// </summary>
        [Fact]
        public async Task EditShouldNotFoundException()
        {
            //Arrange
            var club = TestDataGenerator.Club();
            var coach = TestDataGenerator.Coach();
            var gym = TestDataGenerator.Gym();
            var study = TestDataGenerator.Study();

            await Context.Clubs.AddAsync(club);
            await Context.Coaches.AddAsync(coach);
            await Context.Gyms.AddAsync(gym);
            await Context.Studyes.AddAsync(study);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            var model = TestDataGenerator.TimeTableItemRequestModel();
            model.ClubId = club.Id;
            model.CoachId = coach.Id;
            model.GymId = gym.Id;
            model.StudyId = study.Id;

            //Act
            Func<Task> act = () => timeTableItemService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableEntityNotFoundException<TimeTableItem>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Изменение невалидируемого <see cref="TimeTableItem"/>
        /// </summary>
        [Fact]
        public async Task EditShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.TimeTableItemRequestModel();

            //Act
            Func<Task> act = () => timeTableItemService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение <see cref="TimeTableItem"/>
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            //Arrange
            var club = TestDataGenerator.Club();
            var coach = TestDataGenerator.Coach();
            var gym = TestDataGenerator.Gym();
            var study = TestDataGenerator.Study();

            await Context.Clubs.AddAsync(club);
            await Context.Coaches.AddAsync(coach);
            await Context.Gyms.AddAsync(gym);
            await Context.Studyes.AddAsync(study);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            var timeTableItem = TestDataGenerator.TimeTableItem();
            timeTableItem.ClubId = club.Id;
            timeTableItem.CoachId = coach.Id;
            timeTableItem.GymId = gym.Id;
            timeTableItem.StudyId = study.Id;

            var model = TestDataGenerator.TimeTableItemRequestModel();
            model.Id = timeTableItem.Id;
            model.ClubId = club.Id;
            model.CoachId = coach.Id;
            model.GymId = gym.Id;
            model.StudyId = study.Id;

            await Context.TimeTableItems.AddAsync(timeTableItem);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => timeTableItemService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.TimeTableItems.Single(x => x.Id == timeTableItem.Id);
            entity.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    model.Id,
                    model.StartTime,
                    model.Warning
                });
        }
    }
}

