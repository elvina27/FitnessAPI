namespace Fitness.Services.Contracts.Models
{
    public class StudyModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название занятия
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Описание занятия
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Длительность занятия в минутах
        /// </summary>
        public short Duration { get; set; }
    }
}
