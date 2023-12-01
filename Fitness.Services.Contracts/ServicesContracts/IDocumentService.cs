using Fitness.Services.Contracts.Models;

namespace Fitness.Services.Contracts.ServicesContracts
{
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
    }
}
