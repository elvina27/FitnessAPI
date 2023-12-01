using AutoMapper;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Services.Anchors;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ServicesContracts;

namespace Fitness.Services.Implementations
{
    public class TimeTableItemService : ITimeTableItemService, IServiceAnchor
    {
        private readonly ITimeTableItemReadRepository timeTableItemReadRepository;
        private readonly IMapper mapper;

        private readonly IClubReadRepository clubReadRepository;
        private readonly ICoachReadRepository coachReadRepository;
        private readonly IGymReadRepository gymReadRepository;
        private readonly IStudyReadRepository studyReadRepository;

        public TimeTableItemService(ITimeTableItemReadRepository timeTableItemReadRepository, IMapper mapper, IClubReadRepository clubReadRepository,
            ICoachReadRepository coachReadRepository, IGymReadRepository gymReadRepository, IStudyReadRepository studyReadRepository)
        {
            this.timeTableItemReadRepository = timeTableItemReadRepository;
            this.mapper = mapper;

            this.clubReadRepository = clubReadRepository;
            this.coachReadRepository = coachReadRepository;
            this.gymReadRepository = gymReadRepository;
            this.studyReadRepository = studyReadRepository;
        }

        async Task<IEnumerable<TimeTableItemModel>> ITimeTableItemService.GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await timeTableItemReadRepository.GetAllAsync(cancellationToken);
            var clubs = await clubReadRepository.GetByIdsAsync(result.Select(x => x.ClubId).Distinct(), cancellationToken);
            var coaches = await coachReadRepository.GetByIdsAsync(result.Where(x => x.CoachId.HasValue).Select(x => x.CoachId!.Value).Distinct(), cancellationToken);
            var gyms = await gymReadRepository.GetByIdsAsync(result.Select(x => x.GymId).Distinct(), cancellationToken);
            var studyes = await studyReadRepository.GetByIdsAsync(result.Select(x => x.StudyId).Distinct(), cancellationToken);

            var results = new List<TimeTableItemModel>();

            foreach (var timetableitem in result)
            {
                if (!clubs.TryGetValue(timetableitem.ClubId, out var club) ||
                !gyms.TryGetValue(timetableitem.GymId, out var gym) ||
                !studyes.TryGetValue(timetableitem.StudyId, out var study))
                {
                    continue;
                }
                else
                {
                    var timetableitemModel = mapper.Map<TimeTableItemModel>(timetableitem);

                    timetableitemModel.Club = mapper.Map<ClubModel>(club);
                    timetableitemModel.Coach = timetableitem.CoachId.HasValue &&
                                              coaches.TryGetValue(timetableitem.CoachId!.Value, out var coach)
                        ? mapper.Map<CoachModel>(coach)
                        : null;
                    timetableitemModel.Gym = mapper.Map<GymModel>(gym);
                    timetableitemModel.Study = mapper.Map<StudyModel>(study);

                    results.Add(timetableitemModel);
                }
            }

            return results;
        }

        async Task<TimeTableItemModel?> ITimeTableItemService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await timeTableItemReadRepository.GetByIdAsync(id, cancellationToken);

            if (item == null)
            {
                throw new TimeTableEntityNotFoundException<TimeTableItem>(id);
            }

            var club = await clubReadRepository.GetByIdAsync(item.ClubId, cancellationToken);
            var gym = await gymReadRepository.GetByIdAsync(item.GymId, cancellationToken);
            var study = await studyReadRepository.GetByIdAsync(item.StudyId, cancellationToken);

            var model = mapper.Map<TimeTableItemModel>(item);
            model.Club = mapper.Map<ClubModel>(club);
            model.Gym = mapper.Map<GymModel>(gym);
            model.Study = mapper.Map<StudyModel>(study);
            model.Coach = mapper.Map<CoachModel>(item.CoachId.HasValue
                ? await coachReadRepository.GetByIdAsync(item.CoachId.Value, cancellationToken)
                : null);

            return model;
        }
    }
}
