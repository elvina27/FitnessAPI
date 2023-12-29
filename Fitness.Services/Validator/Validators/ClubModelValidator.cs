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
    /// Валидатор <see cref="ClubModel"/>
    /// </summary>
    public class ClubModelValidator : AbstractValidator<ClubModel>
    {
        public ClubModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .NotNull().WithMessage(MessageForValidation.DefaultMessage)
                .Length(3, 100).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Metro)
                .Length(2, 50).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .NotNull().WithMessage(MessageForValidation.DefaultMessage)
                .Length(3, 200).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage(MessageForValidation.DefaultMessage);
        }
    }
}
