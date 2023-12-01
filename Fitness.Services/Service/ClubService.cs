using AutoMapper;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Services.Anchors;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ServicesContracts;

namespace Fitness.Services.Implementations
{
    public class ClubService : IClubService, IServiceAnchor
    {
        private readonly IClubReadRepository clubReadRepositiry;
        private readonly IMapper mapper;

        public ClubService(IClubReadRepository clubReadRepositiry, IMapper mapper)
        {
            this.clubReadRepositiry = clubReadRepositiry;
            this.mapper = mapper;
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
