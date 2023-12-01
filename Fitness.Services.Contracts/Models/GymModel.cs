using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.Models
{
    public class GymModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название зала
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Вместимость зала
        /// </summary>
        public short Capacity { get; set; }
    }
}
