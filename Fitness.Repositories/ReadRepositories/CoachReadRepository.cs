using Fitness.Common.Entity.InterfaceDB;
using Fitness.Common.Entity.Repositories;
using Fitness.Context.Contracts;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="ICoachReadRepository"/>
    /// </summary>
    public class CoachReadRepository : ICoachReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Reader для связи с бд
        /// </summary>
        private IDbRead reader;

        public CoachReadRepository(IDbRead reader)
        {
            this.reader = reader;
        }

        Task<IReadOnlyCollection<Coach>> ICoachReadRepository.GetAllAsync(CancellationToken cancellationToken)
         => reader.Read<Coach>()
                .NotDeletedAt()
                .OrderBy(x => x.Surname)
                .ToReadOnlyCollectionAsync(cancellationToken);

        Task<Coach?> ICoachReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Coach>()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);

        Task<Dictionary<Guid, Coach>> ICoachReadRepository.GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
         => reader.Read<Coach>()
                .ByIds(ids)
                .OrderBy(x => x.Surname)
                .ToDictionaryAsync(x => x.Id, cancellationToken);
    }
}
