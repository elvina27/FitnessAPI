using Fitness.Context.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Repositories.Contracts.ReadRepositoriesContracts
{
    /// <summary>
    /// Репозиторий чтения <see cref="Coach"/>
    /// </summary>
    public interface ICoachWriteRepository
    {
        // <summary>
        /// Получить список всех <see cref="Coach"/>
        /// </summary>
        Task<IReadOnlyCollection<Coach>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Coach"/> по идентификатору
        /// </summary>
        Task<Coach?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="Coach"/> по идентификаторам
        /// </summary>
        Task<Dictionary<Guid, Coach>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    }
}
