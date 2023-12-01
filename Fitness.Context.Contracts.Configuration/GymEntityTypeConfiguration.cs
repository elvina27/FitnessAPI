using Fitness.Context.Contracts.Configuration;
using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Context.Contracts
{
    public class GymEntityTypeConfiguration : IEntityTypeConfiguration<Gym>
    {
        public void Configure(EntityTypeBuilder<Gym> builder)
        {
            builder.ToTable("Gyms");
            builder.HasIdAsKey();
            builder.PropertyAuditConfiguration();
            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .HasMany(x => x.TimeTableItems)
                .WithOne(x => x.Gym)
                .HasForeignKey(x => x.GymId);

            builder.HasIndex(x => x.Title)
                .IsUnique()
                .HasFilter($"{nameof(Gym.DeletedAt)} is null")
                .HasDatabaseName($"IX_{nameof(Gym)}_{nameof(Gym.Title)}");

            builder.Property(x => x.Capacity)
                .HasMaxLength(3)
                .IsRequired();
        }
    }
}
