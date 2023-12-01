using Fitness.Api.Enums;

namespace Fitnes.API.Models.CreateRequest
{
    /// <summary>
    /// Документ
    /// </summary>
    public class CreateDocumentRequest
    {
        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Серия документа
        /// </summary>
        public string Series { get; set; } = string.Empty;

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public DateTime IssuedAt { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Кем выдан
        /// </summary>
        public string? IssuedBy { get; set; }

        /// <summary>
        /// Тип документов
        /// </summary>
        public DocumentTypesResponse DocumentType { get; set; } = DocumentTypesResponse.None;

        /// <summary>
        /// Идентификатор <see cref="Coach"/>
        /// </summary>
        public Guid? CoachId { get; set; }
    }
}
