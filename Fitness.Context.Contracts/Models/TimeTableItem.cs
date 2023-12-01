using Fitness.Common.Entity.EntityInterface;
using Fitness.Context.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Context.Contracts.Models
{
    /// <summary>
    /// Элемент расписания
    /// </summary>
    public class TimeTableItem : BaseAuditEntity
    {
        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTimeOffset StartTime { get; set; } 

        /// <summary>
        /// Идентификатор занятия
        /// </summary>
        public Guid StudyId { get; set; }
        public Study Study { get; set; }

        /// <summary>
        /// Идентификатор тренера
        /// </summary>
        public Guid? CoachId { get; set; }
        public Coach? Coach { get; set; }

        /// <summary>
        /// Идентификатор зала
        /// </summary>
        public Guid GymId { get; set; }
        public Gym Gym { get; set; }

        /// <summary>
        /// Предупреждение
        /// </summary>
        public string? Warning { get; set; }

        /// <summary>
        /// Идентификатор клуба
        /// </summary>
        public Guid ClubId { get; set; }
        public Club Club { get; set; }
    }
}
