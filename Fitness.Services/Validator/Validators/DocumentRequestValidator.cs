using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;
using FluentValidation;

namespace Fitness.Services.Validator.Validators
{
    /// <summary>
    /// Валидатор <see cref="DocumentModel"/>
    /// </summary>
    public class DocumentRequestValidator : AbstractValidator<DocumentRequestModel>
    {
        public DocumentRequestValidator()
        {

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .NotNull().WithMessage(MessageForValidation.DefaultMessage)
                .Length(3, 50).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Series)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .NotNull().WithMessage(MessageForValidation.DefaultMessage)
                .Length(10, 100).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.IssuedAt)
               .NotNull().WithMessage(MessageForValidation.DefaultMessage)
               .GreaterThan(DateTime.Now.AddMinutes(1)).WithMessage(MessageForValidation.InclusiveBetweenMessage);

            RuleFor(x => x.IssuedBy)
                .Length(10, 100).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.DocumentType).IsInEnum().WithMessage(MessageForValidation.DefaultMessage);

            RuleFor(x => x.CoachId)
               .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
               .When(x => x.CoachId.HasValue);
        }
    }
}

