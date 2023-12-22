using Fitness.General;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;
using Fitness.Services.Validator.Validators;
using FluentValidation;

namespace Fitness.Services.Validator
{
    public sealed class ServicesValidatorService : IServiceValidatorService
    {
        private readonly Dictionary<Type, IValidator> validators = new Dictionary<Type, IValidator>();

        public ServicesValidatorService(IClubReadRepository clubReadRepository, ICoachReadRepository coachReadRepository,
            IGymReadRepository gymReadRepository, IStudyReadRepository studyReadRepository)
        {
            validators.Add(typeof(ClubModel), new ClubModelValidator());
            validators.Add(typeof(CoachModel), new CoachModelValidator());
            validators.Add(typeof(DocumentRequestModel), new DocumentRequestValidator()); 
            validators.Add(typeof(GymModel), new GymModelValidator());
            validators.Add(typeof(StudyModel), new StudyModelValidator());
            validators.Add(typeof(TimeTableItemRequestModel), new TimeTableItemRequestValidator(clubReadRepository,
                coachReadRepository, gymReadRepository, studyReadRepository));
        }

        public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken)
            where TModel : class
        {
            var modelType = model.GetType();
            if (!validators.TryGetValue(modelType, out var validator))
            {
                throw new InvalidOperationException($"Не найден валидатор для модели {modelType}");
            }

            var context = new ValidationContext<TModel>(model);
            var result = await validator.ValidateAsync(context, cancellationToken);

            if (!result.IsValid)
            {
                throw new TimeTableValidationException(result.Errors.Select(x =>
                InvalidateItemModel.New(x.PropertyName, x.ErrorMessage)));
            }
        }
    }
}

