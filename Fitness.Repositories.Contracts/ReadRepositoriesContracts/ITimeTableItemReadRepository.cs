using Fitness.Context.Contracts.Models;

namespace Fitness.Repositories.Contracts.ReadRepositoriesContracts
{
    /// <summary>
    /// Репозиторий чтения <see cref="TimeTableItem"/>
    /// </summary>
    public interface ITimeTableItemReadRepository
    {
        /// <summary>
        /// Получить список всех <see cref="TimeTableItem"/>
        /// </summary>
        Task<IReadOnlyCollection<TimeTableItem>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="TimeTableItem"/> по идентификатору
        /// </summary>
        Task<TimeTableItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Проверить есть ли <see cref="TimeTableItem"/> в коллеции
        /// </summary>
        Task<bool> IsNotNullAsync(Guid id, CancellationToken cancellationToken);
    }
}

