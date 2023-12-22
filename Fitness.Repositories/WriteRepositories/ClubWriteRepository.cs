using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Anchors;
using Fitness.Repositories.Contracts.WriteRepositoriesContracts;

namespace Fitness.Repositories.WriteRepositories
{
    /// <summary>
    /// Реализация <see cref="IClubWriteRepository"/>
    /// </summary>
    public class ClubWriteRepository : BaseWriteRepository<Club> ,IClubWriteRepository, IRepositoryAnchor
    {
        public ClubWriteRepository(IDbWriterContext writerContext)
            : base(writerContext)
        {
        }
    }
}
