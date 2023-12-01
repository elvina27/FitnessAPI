using Fitness.Common.Entity.EntityInterface;
using Fitness.Context.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fitness.Context.Contracts.Models
{
    /// <summary>
    /// Занятие
    /// </summary>
    public class Study : BaseAuditEntity
    {
        /// <summary>
        /// Название занятия
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Описание занятия
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Длительность занятия в минутах
        /// </summary>
        public short Duration { get; set; }

        public Category Category { get; set; } = Category.None;

        public ICollection<TimeTableItem> TimeTableItems { get; set; }

    }
}
