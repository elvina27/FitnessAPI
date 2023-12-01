using Fitness.Common.Entity.InterfaceDB;
using Fitness.Common.Entity.Repositories;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Repositories.ReadRepositories
{
    /// <summary>
    /// Реализация <see cref="ITimeTableItemReadRepository"/>
    /// </summary>
    public class TimeTableItemReadRepository : ITimeTableItemReadRepository, IRepositoryAnchor
    {
        /// <summary>
        /// Reader для связи с бд
        /// </summary>
        private IDbRead reader;

        public TimeTableItemReadRepository(IDbRead reader)
        {
            this.reader = reader;
        }

        Task<IReadOnlyCollection<TimeTableItem>> ITimeTableItemReadRepository.GetAllAsync(CancellationToken cancellationToken)
        => reader.Read<TimeTableItem>()
                .NotDeletedAt()
                .OrderBy(x => x.StartTime)
                .ToReadOnlyCollectionAsync(cancellationToken);

        Task<TimeTableItem?> ITimeTableItemReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<TimeTableItem>()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
