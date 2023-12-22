using Fitness.Context.Contracts;
using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts.Configuration;
using Fitness.Context.Contracts.Configuration.Configurations;

namespace Fitness.Context
{
    /// <summary>
    /// Контекст работы с БД
    /// </summary>
    /// <remarks>
    /// 1) dotnet tool install --global dotnet-ef --version 6.0.0
    /// 2) dotnet tool update --global dotnet-ef
    /// 3) dotnet ef migrations add [name] --project TicketSelling.Context\TicketSelling.Context.csproj
    /// 4) dotnet ef database update --project TicketSelling.Context\TicketSelling.Context.csproj
    /// 5) dotnet ef database update [targetMigrationName] --TicketSelling.Context\TicketSelling.Context.csproj
    /// </remarks>
    public class FitnessContext : DbContext, IFitnessContext, IDbRead, IDbWriter, IUnitOfWork
    {
        public DbSet<Club> Clubs { get; set; }

        public DbSet<Coach> Coaches { get; set; }

        public DbSet<Gym> Gyms { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Study> Studyes { get; set; }

        public DbSet<TimeTableItem> TimeTableItems { get; set; }

        public FitnessContext(DbContextOptions<FitnessContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClubEntityTypeConfiguration).Assembly);
        }

        /// <summary>
        /// Чтение сущностей из БД
        /// </summary>
        IQueryable<TEntity> IDbRead.Read<TEntity>()
            => base.Set<TEntity>()
                .AsNoTracking()
                .AsQueryable();

        /// <summary>
        /// Запись сущности в БД
        /// </summary>
        void IDbWriter.Add<TEntities>(TEntities entity)
            => base.Entry(entity).State = EntityState.Added;

        /// <summary>
        /// Обновление сущностей
        /// </summary>
        void IDbWriter.Update<TEntities>(TEntities entity)
              => base.Entry(entity).State = EntityState.Modified;

        /// <summary>
        /// Удаление сущности из БД
        /// </summary>
        void IDbWriter.Delete<TEntities>(TEntities entity)
              => base.Entry(entity).State = EntityState.Deleted;

        /// <summary>
        /// Сохранение изменений в БД
        /// </summary>
        async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
        {
            var count = await base.SaveChangesAsync(cancellationToken);
            foreach (var entry in base.ChangeTracker.Entries().ToArray())
            {
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            return count;
        }
    }
}