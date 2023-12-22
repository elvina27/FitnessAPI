using AutoMapper;
using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Repositories.Contracts.WriteRepositoriesContracts;
using Fitness.Repositories.ReadRepositories;
using Fitness.Services.Anchors;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;
using Fitness.Services.Contracts.ServicesContracts;
using Fitness.Services.Validator;
using System.IO;
using System.Net.Sockets;

namespace Fitness.Services.Implementations
{
    /// <inheritdoc cref="ITimeTableItemService"/>
    public class TimeTableItemService : ITimeTableItemService, IServiceAnchor
    {
        private readonly ITimeTableItemWriteRepository timeTableItemWriteRepository;
        private readonly ITimeTableItemReadRepository timeTableItemReadRepository;

        private readonly IClubReadRepository clubReadRepository;
        private readonly ICoachReadRepository coachReadRepository;
        private readonly IDocumentReadRepository documentReadRepository;
        private readonly IGymReadRepository gymReadRepository;
        private readonly IStudyReadRepository studyReadRepository;

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IServiceValidatorService validatorService;

        public TimeTableItemService(ITimeTableItemWriteRepository timeTableItemWriteRepository, 
            ITimeTableItemReadRepository timeTableItemReadRepository, IClubReadRepository clubReadRepository,
            ICoachReadRepository coachReadRepository, IDocumentReadRepository documentReadRepository,
            IGymReadRepository gymReadRepository, IStudyReadRepository studyReadRepository,
            IMapper mapper, IUnitOfWork unitOfWork, IServiceValidatorService validatorService)
        {
            this.timeTableItemWriteRepository = timeTableItemWriteRepository;
            this.timeTableItemReadRepository = timeTableItemReadRepository;
            this.clubReadRepository = clubReadRepository;
            this.coachReadRepository = coachReadRepository;
            this.documentReadRepository = documentReadRepository;
            this.gymReadRepository = gymReadRepository;
            this.studyReadRepository = studyReadRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.validatorService = validatorService;
        }

        async Task<TimeTableItemModel> ITimeTableItemService.AddAsync(TimeTableItemRequestModel model, CancellationToken cancellationToken)
        {
            model.Id = Guid.NewGuid();
            await validatorService.ValidateAsync(model, cancellationToken);

            var timeTableItem = mapper.Map<TimeTableItem>(model);
            timeTableItemWriteRepository.Add(timeTableItem);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return await GetTimeTableItemtModelOnMapping(timeTableItem, cancellationToken);
        }

        async Task ITimeTableItemService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var targetTimeTableItem = await timeTableItemReadRepository.GetByIdAsync(id, cancellationToken);

            if (targetTimeTableItem == null)
            {
                throw new TimeTableEntityNotFoundException<TimeTableItem>(id);
            }

            if (targetTimeTableItem.DeletedAt.HasValue)
            {
                throw new TimeTableInvalidOperationException($"Элемент расписания с идентификатором {id} уже удален");
            }

            timeTableItemWriteRepository.Delete(targetTimeTableItem);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<TimeTableItemModel> ITimeTableItemService.EditAsync(TimeTableItemRequestModel model, CancellationToken cancellationToken)
        {
            await validatorService.ValidateAsync(model, cancellationToken);

            var timeTableItem = await timeTableItemReadRepository.GetByIdAsync(model.Id, cancellationToken);

            if (timeTableItem == null)
            {
                throw new TimeTableEntityNotFoundException<TimeTableItem>(model.Id);
            }

            timeTableItem = mapper.Map<TimeTableItem>(model);
            timeTableItemWriteRepository.Update(timeTableItem);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return await GetTimeTableItemtModelOnMapping(timeTableItem, cancellationToken);
        }

        async Task<IEnumerable<TimeTableItemModel>> ITimeTableItemService.GetAllAsync(CancellationToken cancellationToken)
        {
            var timeTableItems = await timeTableItemReadRepository.GetAllAsync(cancellationToken);

            var clubs = await clubReadRepository
                .GetByIdsAsync(timeTableItems.Select(x => x.ClubId).Distinct(), cancellationToken);

            var coaches = await coachReadRepository
                 .GetByIdsAsync(timeTableItems.Where(x => x.CoachId.HasValue).Select(x => x.CoachId!.Value).Distinct(), cancellationToken);

            var studyes = await studyReadRepository
                .GetByIdsAsync(timeTableItems.Select(x => x.StudyId).Distinct(), cancellationToken);

            var gyms = await gymReadRepository
                .GetByIdsAsync(timeTableItems.Select(x => x.GymId).Distinct(), cancellationToken);
           

            var result = new List<TimeTableItemModel>();

            foreach (var timeTableItem in timeTableItems)
            {
                if (!clubs.TryGetValue(timeTableItem.ClubId, out var club) ||
                !gyms.TryGetValue(timeTableItem.GymId, out var gym) ||
                !studyes.TryGetValue(timeTableItem.StudyId, out var study))
                {
                    continue;
                }
                else
                {
                    var timeTableItemModel = mapper.Map<TimeTableItemModel>(timeTableItem);

                    timeTableItemModel.Club = mapper.Map<ClubModel>(club);
                    timeTableItemModel.Coach = timeTableItem.CoachId.HasValue &&
                                              coaches.TryGetValue(timeTableItem.CoachId!.Value, out var document)
                        ? mapper.Map<CoachModel>(document)
                        : null;
                    timeTableItemModel.Gym = mapper.Map<GymModel>(gym);
                    timeTableItemModel.Study = mapper.Map<StudyModel>(study);

                    result.Add(timeTableItemModel);
                }
            }

            return result;
        }

        async Task<TimeTableItemModel?> ITimeTableItemService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await timeTableItemReadRepository.GetByIdAsync(id, cancellationToken);

            if (item == null)
            {
                throw new TimeTableEntityNotFoundException<TimeTableItem>(id);
            }

            return await GetTimeTableItemtModelOnMapping(item, cancellationToken);
        }

        async private Task<TimeTableItemModel> GetTimeTableItemtModelOnMapping(TimeTableItem timeTableItem, CancellationToken cancellationToken)
        {
            var timeTableItemModel = mapper.Map<TimeTableItemModel>(timeTableItem);
            timeTableItemModel.Club = mapper.Map<ClubModel>(await clubReadRepository.GetByIdAsync(timeTableItem.ClubId, cancellationToken));
            timeTableItemModel.Gym = mapper.Map<GymModel>(await gymReadRepository.GetByIdAsync(timeTableItem.GymId, cancellationToken));  
            timeTableItemModel.Coach = mapper.Map<CoachModel>(timeTableItem.CoachId.HasValue
                ? await coachReadRepository.GetByIdAsync(timeTableItem.CoachId.Value, cancellationToken)
                : null);
            timeTableItemModel.Study = mapper.Map<StudyModel>(await studyReadRepository.GetByIdAsync(timeTableItem.StudyId, cancellationToken));

            return timeTableItemModel;
        }
    }
}
