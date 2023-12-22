using Fitness.Services.Contracts.Models;
using FluentValidation;

namespace Fitness.Services.Validator.Validators
{
    /// <summary>
    /// Валидатор <see cref="StudyModel"/>
    /// </summary>
    public class StudyModelValidator : AbstractValidator<StudyModel>
    {
        public StudyModelValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .NotNull().WithMessage(MessageForValidation.DefaultMessage)
                .Length(3, 200).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Description)
                .Length(3, 500).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => (int)x.Duration)
                 .InclusiveBetween(1, 90).WithMessage(MessageForValidation.InclusiveBetweenMessage);

            RuleFor(x => x.Category).IsInEnum().WithMessage(MessageForValidation.DefaultMessage);
        }
    }
}