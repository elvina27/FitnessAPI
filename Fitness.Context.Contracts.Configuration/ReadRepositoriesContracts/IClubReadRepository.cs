using Fitness.Context.Contracts.Models;

namespace Fitness.Repositories.Contracts.ReadRepositoriesContracts
{
    /// <summary>
    /// Репозиторий чтения <see cref="Club"/>
    /// </summary>
    public interface IClubWriteRepository
    {
        /// <summary>
        /// Получить список всех <see cref="Club"/>
        /// </summary>
        Task<IReadOnlyCollection<Club>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Club"/> по идентификатору
        /// </summary>
        Task<Club?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Club"/> по идентификаторам
        /// </summary>
        Task<Dictionary<Guid, Club>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    }
}

