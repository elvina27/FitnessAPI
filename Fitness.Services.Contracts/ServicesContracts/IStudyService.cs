using Fitness.Services.Contracts.Models;

namespace Fitness.Services.Contracts.ServicesContracts
{
    public interface IStudyService
    {
        /// <summary>
        /// Получить список всех <see cref="StudyModel"/>
        /// </summary>
        Task<IEnumerable<StudyModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="StudyModel"/> по идентификатору
        /// </summary>
        Task<StudyModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет новое занятие
        /// </summary>
        Task<StudyModel> AddAsync(StudyModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Редактирует существующее занятие
        /// </summary>
        Task<StudyModel> EditAsync(StudyModel source, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет существующее занятие
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}