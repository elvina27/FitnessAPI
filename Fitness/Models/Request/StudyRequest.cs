using Fitness.API.Models.Response;

namespace Fitness.API.Models.Request
{
    /// <summary>
    ///  Модель запроса создания занятия
    /// </summary>
    public class StudyRequest : StudyResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
    }
}
