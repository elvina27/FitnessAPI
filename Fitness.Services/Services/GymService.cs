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
    /// <inheritdoc cref="IGymService"/>
    public class GymService : IGymService, IServiceAnchor
    {
        private readonly IGymWriteRepository gymWriteRepository;
        private readonly IGymReadRepository gymReadRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IServiceValidatorService validatorService;

        public GymService(IGymWriteRepository gymWriteRepository, IGymReadRepository gymReadRepository,
            IUnitOfWork unitOfWork, IMapper mapper, IServiceValidatorService validatorService)
        {
            this.gymWriteRepository = gymWriteRepository;
            this.gymReadRepository = gymReadRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.validatorService = validatorService;
        }


        async Task<GymModel> IGymService.AddAsync(GymModel model, CancellationToken cancellationToken)
        {
            model.Id = Guid.NewGuid();

            await validatorService.ValidateAsync(model, cancellationToken);

            var item = mapper.Map<Gym>(model);
            gymWriteRepository.Add(item);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<GymModel>(item);
        }

        async Task IGymService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var targetGym = await gymReadRepository.GetByIdAsync(id, cancellationToken);

            if (targetGym == null)
            {
                throw new TimeTableEntityNotFoundException<Gym>(id);
            }

            if (targetGym.DeletedAt.HasValue)
            {
                throw new TimeTableInvalidOperationException($"Зал с идентификатором {id} уже удален");
            }

            gymWriteRepository.Delete(targetGym);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<GymModel> IGymService.EditAsync(GymModel source, CancellationToken cancellationToken)
        {
            await validatorService.ValidateAsync(source, cancellationToken);

            var targetGym = await gymReadRepository.GetByIdAsync(source.Id, cancellationToken);

            if (targetGym == null)
            {
                throw new TimeTableEntityNotFoundException<Gym>(source.Id);
            }

            targetGym = mapper.Map<Gym>(source);
            gymWriteRepository.Update(targetGym);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<GymModel>(targetGym);
        }




        async Task<IEnumerable<GymModel>> IGymService.GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await gymReadRepository.GetAllAsync(cancellationToken);

            return result.Select(x => mapper.Map<GymModel>(x));
        }

        async Task<GymModel?> IGymService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await gymReadRepository.GetByIdAsync(id, cancellationToken);

            if (item == null)
            {
                throw new TimeTableEntityNotFoundException<Gym>(id);
            }

            return mapper.Map<GymModel>(item);
        }
    }
}
