using Fitness.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
