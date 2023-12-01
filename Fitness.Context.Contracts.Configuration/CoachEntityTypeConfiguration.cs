using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness.Context.Contracts.Configuration;

namespace Fitness.Context.Contracts
{
    public class CoachEntityTypeConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.ToTable("Coaches");
            builder.HasIdAsKey();
            builder.PropertyAuditConfiguration();

            builder.Property(x => x.Surname)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Patronymic)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .HasMany(x => x.TimeTableItems)
                .WithOne(x => x.Coach)
                .HasForeignKey(x => x.CoachId);

            builder.HasIndex(x => x.Email)
                .IsUnique()
                .HasFilter($"{nameof(Coach.DeletedAt)} is null")
                .HasDatabaseName($"IX_{nameof(Coach)}_{nameof(Coach.Email)}");

            builder.Property(x => x.Age)
                .HasMaxLength(3)
                .IsRequired();

            builder
                .HasMany(x => x.Documents)
                .WithOne(x => x.Coach)
                .HasForeignKey(x => x.CoachId);
        }
    }
}
