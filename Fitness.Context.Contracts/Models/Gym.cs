using Fitness.Common.Entity.EntityInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fitness.Context.Contracts.Models
{
    /// <summary>
    /// Зал
    /// </summary>
    public class Gym : BaseAuditEntity
    {
        /// <summary>
        /// Название зала
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Вместимость зала
        /// </summary>
        public short Capacity { get; set; }

        public ICollection<TimeTableItem> TimeTableItems { get; set; }
    }
}
