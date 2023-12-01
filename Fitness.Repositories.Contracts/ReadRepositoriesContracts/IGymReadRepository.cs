using Fitness.Context.Contracts.Models;

namespace Fitness.Repositories.Contracts.ReadRepositoriesContracts
{
    /// <summary>
    /// Репозиторий чтения <see cref="Gym"/>
    /// </summary>
    public interface IGymReadRepository
    {
        /// <summary>
        /// Получить список всех <see cref="Gym"/>
        /// </summary>
        Task<IReadOnlyCollection<Gym>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Gym"/> по идентификатору
        /// </summary>
        Task<Gym?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Gym"/> по идентификаторам
        /// </summary>
        Task<Dictionary<Guid, Gym>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    }
}
