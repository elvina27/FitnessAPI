using AutoMapper;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Services.Anchors;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ServicesContracts;

namespace Fitness.Services.Implementations
{
    public class StudyService : IStudyService, IServiceAnchor
    {
        private readonly IStudyReadRepository studyReadRepositiry;
        private readonly IMapper mapper;

        public StudyService(IStudyReadRepository studyReadRepositiry, IMapper mapper)
        {
            this.studyReadRepositiry = studyReadRepositiry;
            this.mapper = mapper;
        }

        async Task<IEnumerable<StudyModel>> IStudyService.GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await studyReadRepositiry.GetAllAsync(cancellationToken);

            return result.Select(x => mapper.Map<StudyModel>(x));
        }

        async Task<StudyModel?> IStudyService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await studyReadRepositiry.GetByIdAsync(id, cancellationToken);

            if (item == null)
            {
                throw new TimeTableEntityNotFoundException<Study>(id);
            }

            return mapper.Map<StudyModel>(item);
        }
    }
}