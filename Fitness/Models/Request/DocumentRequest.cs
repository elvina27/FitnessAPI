using Fitnes.API.Models.CreateRequest;
using Fitnes.API.Models.Response;

namespace Fitness.API.Models.Request
{
    /// <summary>
    ///  Модель запроса создания документа
    /// </summary>
    public class DocumentRequest : CreateDocumentRequest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
    }
}
