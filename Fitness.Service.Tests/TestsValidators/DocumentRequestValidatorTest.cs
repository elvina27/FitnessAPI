using Fitness.Context.Contracts.Enums;
using Fitness.Context.Tests;
using Fitness.Repositories.ReadRepositories;
using Fitness.Services.Contracts.Enums;
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
    public class DocumentRequestValidatorTest : FitnessContextInMemory
    {
        private readonly DocumentRequestValidator validator;

        public DocumentRequestValidatorTest()
        {
            validator = new DocumentRequestValidator();
        }

        /// <summary>
        /// Тест на наличие ошибок
        /// </summary>
        [Fact]
        public async void ValidatorShouldError()
        {
            //Arrange
            var model = TestDataGenerator.DocumentRequestModel(x => { 
                x.Number = "Номер"; 
                x.Series = "Серия"; 
                x.IssuedAt = DateTime.Now;
                x.IssuedBy = "Никем";
                x.DocumentType = (DocumentTypesModel)10;

            });

            model.CoachId = Guid.NewGuid();

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
            var coach = TestDataGenerator.Coach();

            await Context.Coaches.AddAsync(coach);

            await UnitOfWork.SaveChangesAsync(CancellationToken);

            var model = TestDataGenerator.DocumentRequestModel();
            model.CoachId = coach.Id;

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}