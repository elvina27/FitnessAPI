using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.Models
{

    public class TimeTableItemModel
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
        public StudyModel Study { get; set; }

        /// <summary>
        /// Идентификатор тренера
        /// </summary>
        public CoachModel? Coach { get; set; }

        /// <summary>
        /// Идентификатор зала
        /// </summary>
        public GymModel Gym { get; set; }

        /// <summary>
        /// Предупреждение
        /// </summary>
        public string? Warning { get; set; }

        /// <summary>
        /// Идентификатор клуба
        /// </summary>
        public ClubModel Club { get; set; }
    }
}
