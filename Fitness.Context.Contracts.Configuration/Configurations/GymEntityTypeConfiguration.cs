using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Fitness.Context.Contracts.Configuration
{
    /// <summary>
    /// Конфигурация для <see cref="Gym"/>
    /// </summary>
    public class GymEntityTypeConfiguration : IEntityTypeConfiguration<Gym>
    {
        void IEntityTypeConfiguration<Gym>.Configure(EntityTypeBuilder<Gym> builder)
        {
            builder.ToTable("Gyms");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.PropertyAuditConfiguration();

            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.HasIndex(x => x.Title)
               .IsUnique()
               .HasFilter($"{nameof(Gym.DeletedAt)} is null")
               .HasDatabaseName($"IX_{nameof(Gym)}_{nameof(Gym.Title)}");
            builder.Property(x => x.Capacity).HasMaxLength(3).IsRequired();
            builder.HasMany(x => x.TimeTableItems).WithOne(x => x.Gym).HasForeignKey(x => x.GymId);

           

            





            











/*
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
                .IsRequired();*/
        }
    }
}
