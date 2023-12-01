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
    }
}
