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
    /// Тренер
    /// </summary>
    public class Coach : BaseAuditEntity
    {
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

        public ICollection<Document> Documents { get; set; }

        public ICollection<TimeTableItem>? TimeTableItems { get; set; }
    }
}

