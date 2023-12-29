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
    public class ClubModelValidatorTest
    {
        private readonly ClubModelValidator validator;

        public ClubModelValidatorTest()
        {
            validator = new ClubModelValidator();
        }

        /// <summary>
        /// Тест на наличие ошибок
        /// </summary>
        [Fact]
        public void ValidatorShouldError()
        {
            //Arrange
            var model = TestDataGenerator.ClubModel(x => { 
                x.Title = "Название"; 
                x.Metro = "Метро"; 
                x.Title = "Название"; 
                x.Email = "Почта"; });

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        /// <summary>
        /// Тест на отсутствие ошибок
        /// </summary>
        [Fact]
        public void ValidatorShouldSuccess()
        {
            //Arrange
            var model = TestDataGenerator.ClubModel();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
