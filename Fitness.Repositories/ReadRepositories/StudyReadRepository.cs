using Fitness.Common.Entity.InterfaceDB;
using Fitness.Common.Entity.Repositories;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="IStudyReadRepository"/>
    /// </summary>
    public class StudyReadRepository : IStudyReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Контекст для связи с бд
        /// </summary>
        private IDbRead reader;

        public StudyReadRepository(IDbRead reader)
        {
            this.reader = reader;
        }

        Task<IReadOnlyCollection<Study>> IStudyReadRepository.GetAllAsync(CancellationToken cancellationToken)
        => reader.Read<Study>()
                .NotDeletedAt()
                .OrderBy(x => x.Title)
                .ToReadOnlyCollectionAsync(cancellationToken);

        Task<Study?> IStudyReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Study>()
                .ById(id)
                .NotDeletedAt()
                .FirstOrDefaultAsync(cancellationToken);

        Task<Dictionary<Guid, Study>> IStudyReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        => reader.Read<Study>()
                .ByIds(ids)
                .OrderBy(x => x.Title)
                .NotDeletedAt()
                .ToDictionaryAsync(x => x.Id, cancellationToken);
        Task<bool> IStudyReadRepository.IsNotNullAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Study>().NotDeletedAt().AnyAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);
    }
}