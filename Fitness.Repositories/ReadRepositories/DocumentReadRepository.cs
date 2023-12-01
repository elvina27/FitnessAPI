using Fitness.Common.Entity.InterfaceDB;
using Fitness.Common.Entity.Repositories;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="IDocumentReadRepository"/>
    /// </summary>
    public class DocumentReadRepository : IDocumentReadRepository, IRepositoryAnchor
    {
        private IDbRead reader;

        public DocumentReadRepository(IDbRead reader)
        {
            this.reader = reader;
        }

        Task<IReadOnlyCollection<Document>> IDocumentReadRepository.GetAllAsync(CancellationToken cancellationToken)
         => reader.Read<Document>()
                .NotDeletedAt()
                .OrderBy(x => x.Number)
                .ToReadOnlyCollectionAsync(cancellationToken);

        Task<Document?> IDocumentReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Document>()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);

        Task<Dictionary<Guid, Document>> IDocumentReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        => reader.Read<Document>()
                .ByIds(ids)
                .OrderBy(x => x.Number)
                .ToDictionaryAsync(x => x.Id, cancellationToken);
    }
}
