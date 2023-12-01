using Fitness.Common.Entity.InterfaceDB;
using Fitness.Common.Entity.Repositories;
using Fitness.Context.Contracts;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="IStudyWriteRepository"/>
    /// </summary>
    public class StudyReadRepository : IStudyReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Reader для связи с бд
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
                .FirstOrDefaultAsync(cancellationToken);

        Task<Dictionary<Guid, Study>> IStudyReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        => reader.Read<Study>()
                .ByIds(ids)
                .OrderBy(x => x.Title)
                .ToDictionaryAsync(x => x.Id, cancellationToken);
    }
}