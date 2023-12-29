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
                .Length(6, 50).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.Series)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .NotNull().WithMessage(MessageForValidation.DefaultMessage)
                .Length(2, 8).WithMessage(MessageForValidation.LengthMessage);           

            RuleFor(x => x.IssuedBy)
                .Length(10, 100).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.DocumentType).IsInEnum().WithMessage(MessageForValidation.DefaultMessage);

            RuleFor(x => x.CoachId)
               .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
               .When(x => x.CoachId.HasValue);
        }
    }
}

