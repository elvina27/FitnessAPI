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
    }
}