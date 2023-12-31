﻿using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;

namespace Fitness.Services.Contracts.ServicesContracts
{
    /// <summary>
    /// Сервис <see cref="DocumentModel"/>
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Получить список всех <see cref="DocumentModel"/>
        /// </summary>
        Task<IEnumerable<DocumentModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="DocumentModel"/> по идентификатору
        /// </summary>
        Task<DocumentModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет новый кинотетар
        /// </summary>
        Task<DocumentModel> AddAsync(DocumentRequestModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Редактирует существующий кинотеатр
        /// </summary>
        Task<DocumentModel> EditAsync(DocumentRequestModel source, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет существующий кинотетар
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
