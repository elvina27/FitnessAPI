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
    public class StudyModelValidatorTest
    {
        private readonly StudyModelValidator validator;

        public StudyModelValidatorTest()
        {
            validator = new StudyModelValidator();
        }

        /// <summary>
        /// Тест на наличие ошибок
        /// </summary>
        [Fact]
        public void ValidatorShouldError()
        {
            //Arrange
            var model = TestDataGenerator.StudyModel(x => {
                x.Title = "Название";
                x.Description = "Описание";
                x.Duration = -55;
                x.Category = (CategoryModel)6; //!!!!!!!!!!!!!!!!!!!!!!!  как слово сделать?
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
            var model = TestDataGenerator.StudyModel();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
