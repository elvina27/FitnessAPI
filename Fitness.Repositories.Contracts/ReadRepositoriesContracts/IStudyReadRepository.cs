using Fitness.Context.Contracts.Models;

namespace Fitness.Repositories.Contracts.ReadRepositoriesContracts
{
    /// <summary>
    /// Репозиторий чтения <see cref="Study"/>
    /// </summary>
    public interface IStudyReadRepository
    {
        /// <summary>
        /// Получить список всех <see cref="Study"/>
        /// </summary>
        Task<IReadOnlyCollection<Study>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Study"/> по идентификатору
        /// </summary>
        Task<Study?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Study"/> по идентификаторам
        /// </summary>
        Task<Dictionary<Guid, Study>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    }
}