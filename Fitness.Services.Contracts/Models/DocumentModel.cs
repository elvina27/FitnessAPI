using Fitness.Context.Contracts.Enums;

namespace Fitness.Services.Contracts.Models
{
    public class DocumentModel
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
        public DocumentTypesModel DocumentType { get; set; } = DocumentTypesModel.None;

        /// <summary>
        /// Идентификатор <see cref="Coach"/>
        /// </summary>
        public CoachModel? Coach { get; set; }
    }
}
