using Fitness.Common.Entity.Repositories;
using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="IGymReadRepository"/>
    /// </summary>
    public class GymReadRepository : IGymReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Reader для связи с бд
        /// </summary>
        private IDbRead reader;

        public GymReadRepository(IDbRead reader)
        {
            this.reader = reader;
        }

        Task<IReadOnlyCollection<Gym>> IGymReadRepository.GetAllAsync(CancellationToken cancellationToken)
            => reader.Read<Gym>()
                .NotDeletedAt()
                .OrderBy(x => x.Title)
                .ToReadOnlyCollectionAsync(cancellationToken);

        Task<Gym?> IGymReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Gym>()
                .ById(id)
                .NotDeletedAt()
                .FirstOrDefaultAsync(cancellationToken);

        Task<Dictionary<Guid, Gym>> IGymReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
            => reader.Read<Gym>()
                .ByIds(ids)
                .NotDeletedAt()
                .OrderBy(x => x.Title)
                .ToDictionaryAsync(x => x.Id, cancellationToken);
        Task<bool> IGymReadRepository.IsNotNullAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Gym>().NotDeletedAt().AnyAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);
    }
}
