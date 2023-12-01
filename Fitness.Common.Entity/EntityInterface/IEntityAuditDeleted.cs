using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Common.Entity.EntityInterface
{
    /// <summary>
    /// Аудит удаление сущности
    /// </summary>
    public interface IEntityAuditDeleted
    {
        /// <summary>
        /// Дата удаление
        /// </summary>
        public DateTimeOffset? DeletedAt { get; set; }

    }
}
