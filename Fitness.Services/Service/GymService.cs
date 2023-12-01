using AutoMapper;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Services.Anchors;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ServicesContracts;

namespace Fitness.Services.Implementations
{
    public class GymService : IGymService, IServiceAnchor
    {
        private readonly IGymReadRepository gymReadRepositiry;
        private readonly IMapper mapper;

        public GymService(IGymReadRepository gymReadRepositiry, IMapper mapper)
        {
            this.gymReadRepositiry = gymReadRepositiry;
            this.mapper = mapper;
        }

        async Task<IEnumerable<GymModel>> IGymService.GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await gymReadRepositiry.GetAllAsync(cancellationToken);

            return result.Select(x => mapper.Map<GymModel>(x));
        }

        async Task<GymModel?> IGymService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await gymReadRepositiry.GetByIdAsync(id, cancellationToken);

            if (item == null)
            {
                throw new TimeTableEntityNotFoundException<Gym>(id);
            }

            return mapper.Map<GymModel>(item);
        }
    }
}
