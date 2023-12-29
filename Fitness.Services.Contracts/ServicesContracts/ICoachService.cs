using Fitness.Services.Contracts.Models;

namespace Fitness.Services.Contracts.ServicesContracts
{
    public interface ICoachService
    {
        /// <summary>
        /// Получить список всех <see cref="CoachModel"/>
        /// </summary>
        Task<IEnumerable<CoachModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="CoachModel"/> по идентификатору
        /// </summary>
        Task<CoachModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет нового тренера
        /// </summary>
        Task<CoachModel> AddAsync(CoachModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Редактирует существующего тренера
        /// </summary>
        Task<CoachModel> EditAsync(CoachModel source, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет существующего тренера
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
