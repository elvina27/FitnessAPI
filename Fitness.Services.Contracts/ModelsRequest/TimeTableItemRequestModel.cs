using Fitness.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.ModelsRequest
{
    /// <summary>
    /// Модель запроса создания элемента расписания
    /// </summary>
    public class TimeTableItemRequestModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// Идентификатор занятия
        /// </summary>
        public Guid StudyId { get; set; }

        /// <summary>
        /// Идентификатор тренера
        /// </summary>
        public Guid CoachId { get; set; }

        /// <summary>
        /// Идентификатор зала
        /// </summary>
        public Guid GymId { get; set; }

        /// <summary>
        /// Предупреждение
        /// </summary>
        public string? Warning { get; set; }

        /// <summary>
        /// Идентификатор клуба
        /// </summary>
        public Guid ClubId { get; set; }   
    }
}
