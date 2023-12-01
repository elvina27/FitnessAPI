using Fitness.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.ServicesContracts
{
    public interface ITimeTableItemService
    {
        /// <summary>
        /// Получить список всех <see cref="TimeTableItemModel"/>
        /// </summary>
        Task<IEnumerable<TimeTableItemModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="TimeTableItemModel"/> по идентификатору
        /// </summary>
        Task<TimeTableItemModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
