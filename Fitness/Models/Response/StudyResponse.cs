using Fitness.Api.Enums;

namespace Fitness.API.Models.Response
{
    /// <summary>
    /// Занятие
    /// </summary>
    public class StudyResponse
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

        public CategoryResponse Category { get; set; } = CategoryResponse.None;

    }
}
