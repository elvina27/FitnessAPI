using AutoMapper;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;

using Fitness.Services.Contracts.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Implementations
{
    public class CoachService : ICoachService
    {
        private readonly ICoachReadRepository coachReadRepository;
        private readonly IMapper mapper;

        public CoachService(ICoachReadRepository coachReadRepository, IMapper mapper)
        {
            this.coachReadRepository = coachReadRepository;
            this.mapper = mapper;
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
