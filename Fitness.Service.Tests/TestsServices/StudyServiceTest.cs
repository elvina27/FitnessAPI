using AutoMapper;
using Fitness.Context.Contracts.Models;
using Fitness.Context.Tests;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fitness.Services.Tests.TestsServices
{
    public class StudyServiceTest : FitnessContextInMemory
    {
        private readonly IStudyService studyService;
        private readonly StudyReadRepository studyReadRepository;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="StudyServiceTest"/>
        /// </summary>
        public StudyServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ServiceMapper());
            });

            studyReadRepository = new StudyReadRepository(Reader);

            studyService = new StudyService(new StudyWriteRepository(WriterContext), new StudyReadRepository(Reader),
                UnitOfWork, config.CreateMapper(), new ServicesValidatorService(new ClubReadRepository(Reader),
                    new CoachReadRepository(Reader), new GymReadRepository(Reader), studyReadRepository));
        }

        /// <summary>
        /// Получение <see cref="Study"/> по идентификатору возвращает null
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => studyService.GetByIdAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Study>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Получение <see cref="Study"/> по идентификатору возвращает данные
        /// </summary>
        [Fact]
        public async Task GetByIdShouldReturnValue()
        {
            //Arrange
            var target = TestDataGenerator.Study();
            await Context.Studyes.AddAsync(target);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await studyService.GetByIdAsync(target.Id, CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(new
                {
                    target.Id,
                    target.Title,
                    target.Description,
                    target.Duration //////////////////!!!!!!!!!!!!!!!!!!!!!!нужно ли Category????
                });
        }

        /// <summary>
        /// Получение <see cref="IEnumerable{Study}"/> по идентификаторам возвращает пустую коллекцию
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnEmpty()
        {
            // Act
            var result = await studyService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.BeEmpty();
        }

        /// <summary>
        /// Получение <see cref="Study"/> по идентификаторам возвращает данные
        /// </summary>
        [Fact]
        public async Task GetAllShouldReturnValues()
        {
            //Arrange
            var target = TestDataGenerator.Study();

            await Context.Studyes.AddRangeAsync(target,
                TestDataGenerator.Study(x => x.DeletedAt = DateTimeOffset.UtcNow));
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            var result = await studyService.GetAllAsync(CancellationToken);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == target.Id);
        }

        /// <summary>
        /// Удаление несуществуюущего <see cref="Study"/>
        /// </summary>
        [Fact]
        public async Task DeletingNonExistentStudyReturnExсeption()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> result = () => studyService.DeleteAsync(id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Study>>()
                .WithMessage($"*{id}*");
        }

        /// <summary>
        /// Удаление удаленного <see cref="Study"/>
        /// </summary>
        [Fact]
        public async Task DeletingDeletedStudyReturnExсeption()
        {
            //Arrange
            var model = TestDataGenerator.Study(x => x.DeletedAt = DateTime.UtcNow);
            await Context.Studyes.AddAsync(model);
            await Context.SaveChangesAsync(CancellationToken);

            // Act
            Func<Task> result = () => studyService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await result.Should().ThrowAsync<TimeTableEntityNotFoundException<Study>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Удаление <see cref="Study"/>
        /// </summary>
        [Fact]
        public async Task DeleteShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.Study();
            await Context.Studyes.AddAsync(model);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => studyService.DeleteAsync(model.Id, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Studyes.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().NotBeNull();
        }

        /// <summary>
        /// Добавление <see cref="Study"/>
        /// </summary>
        [Fact]
        public async Task AddShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.StudyModel();

            //Act
            Func<Task> act = () => studyService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Studyes.Single(x => x.Id == model.Id);
            entity.Should().NotBeNull();
            entity.DeletedAt.Should().BeNull();
        }

        /// <summary>
        /// Добавление не валидируемого <see cref="Study"/>
        /// </summary>
        [Fact]
        public async Task AddShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.StudyModel(x => x.Title = "w");

            //Act
            Func<Task> act = () => studyService.AddAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение несуществующего <see cref="Study"/>
        /// </summary>
        [Fact]
        public async Task EditShouldNotFoundException()
        {
            //Arrange
            var model = TestDataGenerator.StudyModel();

            //Act
            Func<Task> act = () => studyService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableEntityNotFoundException<Study>>()
                .WithMessage($"*{model.Id}*");
        }

        /// <summary>
        /// Изменение невалидируемого <see cref="Study"/>
        /// </summary>
        [Fact]
        public async Task EditShouldValidationException()
        {
            //Arrange
            var model = TestDataGenerator.StudyModel(x => x.Title = "w");

            //Act
            Func<Task> act = () => studyService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().ThrowAsync<TimeTableValidationException>();
        }

        /// <summary>
        /// Изменение <see cref="Study"/>
        /// </summary>
        [Fact]
        public async Task EditShouldWork()
        {
            //Arrange
            var model = TestDataGenerator.StudyModel();
            var study = TestDataGenerator.Study(x => x.Id = model.Id);
            await Context.Studyes.AddAsync(study);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            //Act
            Func<Task> act = () => studyService.EditAsync(model, CancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            var entity = Context.Studyes.Single(x => x.Id == study.Id);
            entity.Should().NotBeNull()
                .And
                .BeEquivalentTo(new
                {
                    model.Id,
                    model.Title,
                    model.Description,
                    model.Duration
                });
        }
    }
}

