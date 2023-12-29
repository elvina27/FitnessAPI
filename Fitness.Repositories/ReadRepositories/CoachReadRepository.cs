using Fitness.Common.Entity.InterfaceDB;
using Fitness.Common.Entity.Repositories;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="ICoachReadRepository"/>
    /// </summary>
    public class CoachReadRepository : ICoachReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Reader для связи с бд
        /// </summary>
        private IDbRead reader;

        public CoachReadRepository(IDbRead reader)
        {
            this.reader = reader;
        }

        Task<IReadOnlyCollection<Coach>> ICoachReadRepository.GetAllAsync(CancellationToken cancellationToken)
         => reader.Read<Coach>()
                .NotDeletedAt()
                .OrderBy(x => x.Surname)
                .ToReadOnlyCollectionAsync(cancellationToken);

        Task<Coach?> ICoachReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Coach>()
                .ById(id)
                .NotDeletedAt()
                .FirstOrDefaultAsync(cancellationToken);
                

        Task<Dictionary<Guid, Coach>> ICoachReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
         => reader.Read<Coach>()
                .ByIds(ids)
                .NotDeletedAt()
                .OrderBy(x => x.Surname)
                .ToDictionaryAsync(x => x.Id, cancellationToken);
        Task<bool> ICoachReadRepository.IsNotNullAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Coach>().NotDeletedAt().AnyAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);
    }
}
