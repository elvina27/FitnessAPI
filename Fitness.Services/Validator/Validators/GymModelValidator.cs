using Fitness.Services.Contracts.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Validator.Validators
{
    /// <summary>
    /// Валидатор <see cref="GymModel"/>
    /// </summary>
    public class GymModelValidator : AbstractValidator<GymModel>
    {
        public GymModelValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .NotNull().WithMessage(MessageForValidation.DefaultMessage)
                .Length(10, 100).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => (int)x.Capacity)
             .InclusiveBetween(1, 50).WithMessage(MessageForValidation.InclusiveBetweenMessage);           
        }
    }
}

