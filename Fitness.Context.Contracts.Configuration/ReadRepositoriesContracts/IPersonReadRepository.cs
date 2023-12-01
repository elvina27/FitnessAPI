using Fitness.Context.Contracts.Models;

namespace Fitness.Repositories.Contracts.ReadRepositoriesContracts
{
    /// <summary>
    /// Репозиторий чтения <see cref="Document"/>
    /// </summary>
    public interface IPersonWriteRepository
    {
        /// <summary>
        /// Получить список всех <see cref="Document"/>
        /// </summary>
        Task<IReadOnlyCollection<Document>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Document"/> по идентификатору
        /// </summary>
        Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Document"/> по идентификаторам
        /// </summary>
        Task<Dictionary<Guid, Document>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    }
}
