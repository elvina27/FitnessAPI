using Fitness.API.Models.Response;

namespace Fitness.API.Models.Request
{
    /// <summary>
    ///  Модель запроса создания клуба
    /// </summary>
    public class ClubRequest : ClubResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

    }
}
