//using Fitness.Context.Contracts.Enums;
using Fitness.Services.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.ModelsRequest
{
    /// <summary>
    /// Модель запроса создания документа
    /// </summary>
    public class DocumentRequestModel
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
        public DateTime IssuedAt { get; set; } 

        /// <summary>
        /// Кем выдан
        /// </summary>
        public string? IssuedBy { get; set; }

        /// <summary>
        /// Тип документов
        /// </summary>
        public DocumentTypesModel DocumentType { get; set; } = DocumentTypesModel.None;

        /// <summary>
        /// Идентификатор тренера
        /// </summary>
        public Guid? CoachId { get; set; }


    }
}
