namespace Fitness.API.Models.Response
{
    /// <summary>
    /// Элемент расписания
    /// </summary>
    public class TimeTableItemResponse
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
        public StudyResponse Study { get; set; }

        /// <summary>
        /// Идентификатор тренера
        /// </summary>
        public CoachResponse? Coach { get; set; }

        /// <summary>
        /// Идентификатор зала
        /// </summary>
        public GymResponse Gym { get; set; }

        /// <summary>
        /// Предупреждение
        /// </summary>
        public string? Warning { get; set; }

        /// <summary>
        /// Идентификатор клуба
        /// </summary>
        public ClubResponse Club { get; set; }
    }
}
