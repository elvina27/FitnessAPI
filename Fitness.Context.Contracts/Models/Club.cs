using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fitness.Context.Contracts.Models
{
    /// <summary>
    /// Клуб
    /// </summary>
    public class Club : BaseAuditEntity
    {
        /// <summary>
        /// Название клуба
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Станция метро 
        /// </summary>
        public string? Metro { get; set; }

        /// <summary>
        /// Адрес 
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; } = string.Empty;

        public ICollection<TimeTableItem> TimeTableItems { get; set; }

    }
}
