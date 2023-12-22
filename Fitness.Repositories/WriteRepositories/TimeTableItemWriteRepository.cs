using Fitness.Common.Entity.InterfaceDB;
using Fitness.Common.Entity.Repositories;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Repositories.Contracts.WriteRepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Repositories.WriteRepositories
{
    /// <summary>
    /// Реализация <see cref="ITimeTableItemWriteRepository"/>
    /// </summary>
    public class TimeTableItemWriteRepository : BaseWriteRepository<TimeTableItem>, ITimeTableItemWriteRepository, IRepositoryAnchor
    {
        public TimeTableItemWriteRepository(IDbWriterContext writerContext)
            : base(writerContext)
        {
        }
    }
}
