using Fitness.API.Models.CreateRequest;
using Fitness.API.Models.Response;

namespace Fitness.API.Models.Request
{
    /// <summary>
    ///  Модель запроса создания элемента расписания
    /// </summary>
    public class TimeTableItemRequest : CreateTimeTableItemRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
    }
}
