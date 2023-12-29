using Fitness.Services.Contracts.Models;
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
    public class CoachModelValidatorTest
    {
        private readonly CoachModelValidator validator;

        public CoachModelValidatorTest()
        {
            validator = new CoachModelValidator();
        }

        /// <summary>
        /// Тест на наличие ошибок
        /// </summary>
        [Fact]
        public void ValidatorShouldError()
        {
            //Arrange
            var model = TestDataGenerator.CoachModel(x => {
                x.Surname = "Фамилия";
                x.Name = "Имя";
                x.Patronymic = "Отчество";
                x.Email = "Почта";
                x.Age = -35;
            });

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
            var model = TestDataGenerator.CoachModel();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
