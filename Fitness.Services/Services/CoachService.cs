using AutoMapper;
using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Repositories.Contracts.WriteRepositoriesContracts;
using Fitness.Services.Anchors;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;

using Fitness.Services.Contracts.ServicesContracts;
using Fitness.Services.Validator;

namespace Fitness.Services.Implementations
{
    /// <inheritdoc cref="ICoachService"/>
    public class CoachService : ICoachService, IServiceAnchor
    {
        private readonly ICoachWriteRepository coachWriteRepository;
        private readonly ICoachReadRepository coachReadRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IServiceValidatorService validatorService;

        public CoachService(ICoachWriteRepository coachWriteRepository, ICoachReadRepository coachReadRepository,
            IUnitOfWork unitOfWork, IMapper mapper, IServiceValidatorService validatorService)
        {
            this.coachReadRepository = coachReadRepository;
            this.coachWriteRepository = coachWriteRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.validatorService = validatorService; 
        }
        async Task<CoachModel> ICoachService.AddAsync(CoachModel model, CancellationToken cancellationToken)
        {
            model.Id = Guid.NewGuid();

            await validatorService.ValidateAsync(model, cancellationToken);

            var item = mapper.Map<Coach>(model);
            coachWriteRepository.Add(item);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<CoachModel>(item);
        }

        async Task ICoachService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var targetCoach = await coachReadRepository.GetByIdAsync(id, cancellationToken);

            if (targetCoach == null)
            {
                throw new TimeTableEntityNotFoundException<Coach>(id);
            }

            if (targetCoach.DeletedAt.HasValue)
            {
                throw new TimeTableInvalidOperationException($"Тренер с идентификатором {id} уже удален");
            }

            coachWriteRepository.Delete(targetCoach);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<CoachModel> ICoachService.EditAsync(CoachModel source, CancellationToken cancellationToken)
        {
            await validatorService.ValidateAsync(source, cancellationToken);

            var targetCoach = await coachReadRepository.GetByIdAsync(source.Id, cancellationToken);

            if (targetCoach == null)
            {
                throw new TimeTableEntityNotFoundException<Coach>(source.Id);
            }

            targetCoach = mapper.Map<Coach>(source);
            coachWriteRepository.Update(targetCoach);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<CoachModel>(targetCoach);
        }

        async Task<IEnumerable<CoachModel>> ICoachService.GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await coachReadRepository.GetAllAsync(cancellationToken);

            return result.Select(x => mapper.Map<CoachModel>(x));
        }

        async Task<CoachModel?> ICoachService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await coachReadRepository.GetByIdAsync(id, cancellationToken);

            if (item == null)
            {
                throw new TimeTableEntityNotFoundException<Coach>(id);
            }

            return mapper.Map<CoachModel>(item);
        }
    }
}
