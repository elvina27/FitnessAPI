using Fitness.Api.Enums;
using Fitness.API.Models.Response;

namespace Fitnes.API.Models.Response
{
    /// <summary>
    /// Документ
    /// </summary>
    public class DocumentResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

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
        /// Идентификатор <see cref="CoachResponse"/>
        /// </summary>
        public CoachResponse? Coach { get; set; }
    }
}
