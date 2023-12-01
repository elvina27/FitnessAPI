using System.Text.RegularExpressions;
using System;
using Fitness.Context.Contracts;
using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Fitness.Common.Entity.InterfaceDB;

namespace Fitness.Context
{
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

        IQueryable<TEntity> IDbRead.Read<TEntity>()
            => base.Set<TEntity>()
                .AsNoTracking()
                .AsQueryable();

        void IDbWriter.Add<TEntities>(TEntities entity)
            => base.Entry(entity).State = EntityState.Added;

        void IDbWriter.Update<TEntities>(TEntities entity)
              => base.Entry(entity).State = EntityState.Modified;

        void IDbWriter.Delete<TEntities>(TEntities entity)
              => base.Entry(entity).State = EntityState.Deleted;


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