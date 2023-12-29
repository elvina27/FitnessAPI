using Fitness.Context.Tests;
using Fitness.Repositories.ReadRepositories;
using Fitness.Services.Validator.Validators;
using Fitness.Tests.Extensions;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fitness.Services.Tests.TestsValidators
{
    public class TimeTableItemRequestValidatorTest : FitnessContextInMemory
    {
        private readonly TimeTableItemRequestValidator validator;

        public TimeTableItemRequestValidatorTest()
        {
            validator = new TimeTableItemRequestValidator(new ClubReadRepository(Reader), 
                new CoachReadRepository(Reader), new GymReadRepository(Reader), new StudyReadRepository(Reader));
        }

        /// <summary>
        /// Тест на наличие ошибок
        /// </summary>
        [Fact]
        public async void ValidatorShouldError()
        {
            //Arrange
            var model = TestDataGenerator.TimeTableItemRequestModel(
                x => {
                    x.StartTime = DateTimeOffset.Now; 
                    x.Warning = "Предупреждение"; 
                    });

            model.ClubId = Guid.NewGuid();
            model.CoachId = Guid.NewGuid();
            model.GymId = Guid.NewGuid();
            model.StudyId = Guid.NewGuid();

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        /// <summary>
        /// Тест на отсутствие ошибок
        /// </summary>
        [Fact]
        async public void ValidatorShouldSuccess()
        {
            //Arrange
            var clud = TestDataGenerator.Club();
            var coach = TestDataGenerator.Coach();
            var gyms = TestDataGenerator.Gym();
            var study = TestDataGenerator.Study();

            await Context.Clubs.AddAsync(clud);
            await Context.Coaches.AddAsync(coach);
            await Context.Gyms.AddAsync(gyms);
            await Context.Studyes.AddAsync(study);
            await UnitOfWork.SaveChangesAsync(CancellationToken);

            var model = TestDataGenerator.TimeTableItemRequestModel();
            model.ClubId = clud.Id;
            model.CoachId = coach.Id;
            model.GymId = gyms.Id;
            model.StudyId = study.Id;

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
