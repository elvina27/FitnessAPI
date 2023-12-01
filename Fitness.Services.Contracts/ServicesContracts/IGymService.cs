using Fitness.Services.Contracts.Models;

namespace Fitness.Services.Contracts.ServicesContracts
{
    public interface IGymService
    {
        /// <summary>
        /// Получить список всех <see cref="GymModel"/>
        /// </summary>
        Task<IEnumerable<GymModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="GymModel"/> по идентификатору
        /// </summary>
        Task<GymModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
