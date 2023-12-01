using Fitness.API.Models.Response;

namespace Fitness.API.Models.Request
{
    /// <summary>
    ///  Модель запроса создания зала
    /// </summary>
    public class GymRequest : GymResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
    }
}
