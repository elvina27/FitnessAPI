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
    /// <inheritdoc cref="IClubService"/> 
    public class ClubService : IClubService, IServiceAnchor
    {
        private readonly IClubReadRepository clubReadRepositiry;
        private readonly IClubWriteRepository clubWriteRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IServiceValidatorService validatorService;
        public ClubService(IClubWriteRepository clubWriteRepository, IClubReadRepository clubReadRepositiry,
            IUnitOfWork unitOfWork, IMapper mapper, IServiceValidatorService validatorService)
        {
            this.clubReadRepositiry = clubReadRepositiry;
            this.mapper = mapper;
            this.clubWriteRepository = clubWriteRepository;
            this.unitOfWork = unitOfWork;
            this.validatorService = validatorService;
        }

        async Task<ClubModel> IClubService.AddAsync(ClubModel model, CancellationToken cancellationToken)
        {
            model.Id = Guid.NewGuid();

            await validatorService.ValidateAsync(model, cancellationToken);

            var item = mapper.Map<Club>(model);
            clubWriteRepository.Add(item);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ClubModel>(item);
        }

        async Task IClubService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var targetClub = await clubReadRepositiry.GetByIdAsync(id, cancellationToken);

            if (targetClub == null)
            {
                throw new TimeTableEntityNotFoundException<Club>(id);
            }

            if (targetClub.DeletedAt.HasValue)
            {
                throw new TimeTableInvalidOperationException($"Клуб с идентификатором {id} уже удален");
            }

            clubWriteRepository.Delete(targetClub);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<ClubModel> IClubService.EditAsync(ClubModel source, CancellationToken cancellationToken)
        {
            await validatorService.ValidateAsync(source, cancellationToken);

            var targetClub = await clubReadRepositiry.GetByIdAsync(source.Id, cancellationToken);

            if (targetClub == null)
            {
                throw new TimeTableEntityNotFoundException<Club>(source.Id);
            }

            targetClub = mapper.Map<Club>(source);
            clubWriteRepository.Update(targetClub);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ClubModel>(targetClub);
        }

        async Task<IEnumerable<ClubModel>> IClubService.GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await clubReadRepositiry.GetAllAsync(cancellationToken);

            return result.Select(x => mapper.Map<ClubModel>(x));
        }

        async Task<ClubModel?> IClubService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await clubReadRepositiry.GetByIdAsync(id, cancellationToken);

            if (item == null)
            {
                throw new TimeTableEntityNotFoundException<Club>(id);
            }

            return mapper.Map<ClubModel>(item);
        }
    }
}
