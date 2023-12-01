using Fitness.Context.Contracts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
    }
}

