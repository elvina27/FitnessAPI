using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.Models
{
    public class ClubModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

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
    }
}
