using Fitness.Common.Entity.InterfaceDB;
using Fitness.Common.Entity.Repositories;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="IClubReadRepository"/>
    /// </summary>
    public class ClubReadRepository : IClubReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Reader для связи с бд
        /// </summary>
        private IDbRead reader;

        public ClubReadRepository(IDbRead reader)
        {
            this.reader = reader;
        }

        Task<IReadOnlyCollection<Club>> IClubReadRepository.GetAllAsync(CancellationToken cancellationToken)       
             => reader.Read<Club>()
                .NotDeletedAt()
                .OrderBy(x => x.Title)
                .ToReadOnlyCollectionAsync(cancellationToken);      

        Task<Club?> IClubReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
             => reader.Read<Club>()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);

        Task<Dictionary<Guid, Club>> IClubReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
             => reader.Read<Club>()
                .ByIds(ids)
                .OrderBy(x => x.Title)
                .ToDictionaryAsync(x => x.Id, cancellationToken);
        Task<bool> IClubReadRepository.IsNotNullAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Club>().AnyAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);
    }
}
