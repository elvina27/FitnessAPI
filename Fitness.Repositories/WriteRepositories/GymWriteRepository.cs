using Fitness.Common.Entity.Repositories;
using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Fitness.Repositories.Contracts.WriteRepositoriesContracts;

namespace Fitness.Repositories.WriteRepositories
{
    /// <summary>
    /// Реализация <see cref="IGymWriteRepository"/>
    /// </summary>
    public class GymWriteRepository : BaseWriteRepository<Gym>, IGymWriteRepository, IRepositoryAnchor
    {
        public GymWriteRepository(IDbWriterContext writerContext)
            : base(writerContext)
        {
        }
    }
}
