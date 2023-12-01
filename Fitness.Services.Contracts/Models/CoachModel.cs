using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.Models
{
    public class CoachModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Фамилия 
        /// </summary>
        public string Surname { get; set; } = string.Empty;

        /// <summary>
        /// Имя 
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Отчество 
        /// </summary>
        public string Patronymic { get; set; } = string.Empty;

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Возраст
        /// </summary>
        public short Age { get; set; }
    }
}
