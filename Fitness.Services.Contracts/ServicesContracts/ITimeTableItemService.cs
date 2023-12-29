﻿using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;

namespace Fitness.Services.Contracts.ServicesContracts
{
    /// <summary>
    /// Сервис <see cref="TimeTableItemModel"/>
    /// </summary>
    public interface ITimeTableItemService
    {
        /// <summary>
        /// Получить список всех <see cref="TimeTableItemModel"/>
        /// </summary>
        Task<IEnumerable<TimeTableItemModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="TimeTableItemModel"/> по идентификатору
        /// </summary>
        Task<TimeTableItemModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет новый элемент расписания
        /// </summary>
        Task<TimeTableItemModel> AddAsync(TimeTableItemRequestModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Редактирует существующий элемент расписания
        /// </summary>
        Task<TimeTableItemModel> EditAsync(TimeTableItemRequestModel source, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет существующий элемент расписания
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
