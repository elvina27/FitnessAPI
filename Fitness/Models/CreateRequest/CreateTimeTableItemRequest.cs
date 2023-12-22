namespace Fitness.API.Models.CreateRequest
{
    /// <summary>
    /// Модель запроса создания элемента расписания
    /// </summary>
    public class CreateTimeTableItemRequest
    {
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
        public Guid? CoachId { get; set; }

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
