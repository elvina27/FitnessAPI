using Fitness.Services.Contracts.Models;

namespace Fitness.Services.Contracts.ServicesContracts
{
    public interface IClubService
    {
        /// <summary>
        /// Получить список всех <see cref="ClubModel"/>
        /// </summary>
        Task<IEnumerable<ClubModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="ClubModel"/> по идентификатору
        /// </summary>
        Task<ClubModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет новый клуб
        /// </summary>
        Task<ClubModel> AddAsync(ClubModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Редактирует существующий клуб
        /// </summary>
        Task<ClubModel> EditAsync(ClubModel source, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет существующий клуб
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
