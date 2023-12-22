using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Validator.Validators
{
    /// <summary>
    /// Валидатор <see cref="TimeTableItemModel"/>
    /// </summary>
    internal class TimeTableItemRequestValidator : AbstractValidator<TimeTableItemRequestModel>
    {
        private readonly IClubReadRepository clubReadRepository;
        private readonly ICoachReadRepository coachReadRepository;
        private readonly IGymReadRepository gymReadRepository;
        private readonly IStudyReadRepository studyReadRepository;

        public TimeTableItemRequestValidator(IClubReadRepository clubReadRepository, ICoachReadRepository coachReadRepository,
            IGymReadRepository gymReadRepository, IStudyReadRepository studyReadRepository)
        {
            this.clubReadRepository = clubReadRepository;
            this.coachReadRepository = coachReadRepository;
            this.gymReadRepository = gymReadRepository;
            this.studyReadRepository = studyReadRepository;

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .GreaterThan(DateTimeOffset.Now.AddMinutes(1)).WithMessage(MessageForValidation.InclusiveBetweenMessage);///

            RuleFor(x => x.StudyId)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .MustAsync(async (x, cancellationToken) => await this.studyReadRepository.IsNotNullAsync(x, cancellationToken))
                .WithMessage(MessageForValidation.NotFoundGuidMessage);

            RuleFor(x => x.CoachId)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .MustAsync(async (x, cancellationToken) => await this.coachReadRepository.IsNotNullAsync(x, cancellationToken))
                .WithMessage(MessageForValidation.NotFoundGuidMessage);

            RuleFor(x => x.GymId)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .MustAsync(async (x, cancellationToken) => await this.gymReadRepository.IsNotNullAsync(x, cancellationToken))
                .WithMessage(MessageForValidation.NotFoundGuidMessage);

            RuleFor(x => x.Warning)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .Length(3, 500).WithMessage(MessageForValidation.LengthMessage);

            RuleFor(x => x.ClubId)
                .NotEmpty().WithMessage(MessageForValidation.DefaultMessage)
                .MustAsync(async (x, cancellationToken) => await this.clubReadRepository.IsNotNullAsync(x, cancellationToken))
                .WithMessage(MessageForValidation.NotFoundGuidMessage);
        }
    }
}
