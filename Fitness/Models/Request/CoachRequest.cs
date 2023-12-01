using Fitness.API.Models.CreateRequest;

namespace Fitness.API.Models.Request
{
    /// <summary>
    ///  Модель запроса создания тренера
    /// </summary>
    public class CoachRequest : CreateCoachRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
    }
}

