using Fitness.Context.Contracts.Configuration;
using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Fitness.Context.Contracts.Configuration.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="Club"/>
    /// </summary>
    public class ClubEntityTypeConfiguration : IEntityTypeConfiguration<Club>
    {
        void IEntityTypeConfiguration<Club>.Configure(EntityTypeBuilder<Club> builder)
        {
            builder.ToTable("Clubs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.PropertyAuditConfiguration();
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.HasIndex(x => x.Title)
                .IsUnique()
                .HasDatabaseName($"{nameof(Club)}_{nameof(Club.Email)}")
                .HasFilter($"{nameof(Club.DeletedAt)} is null");
            builder.Property(x => x.Metro).HasMaxLength(30); 
            builder.Property(x => x.Address).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired();                  
            builder.HasMany(x => x.TimeTableItems).WithOne(x => x.Club).HasForeignKey(x => x.ClubId);






            /*
            builder.ToTable("Clubs");
             builder.HasIdAsKey();
             builder.PropertyAuditConfiguration();

             builder.Property(x => x.Title)
                 .HasMaxLength(100)
                 .IsRequired();

             builder.Property(x => x.Metro)
                 .HasMaxLength(30);

             builder.Property(x => x.Address)
                 .HasMaxLength(200)
                 .IsRequired();

             builder.Property(x => x.Email)
                 .HasMaxLength(100)
                 .IsRequired();

             builder.HasIndex(x => x.Title)
                 .IsUnique()
                 .HasFilter($"{nameof(Club.DeletedAt)} is null")
                 .HasDatabaseName($"IX_{nameof(Club)}_{nameof(Club.Title)}");

             builder.HasMany(x => x.TimeTableItems)
                 .WithOne(x => x.Club)
                 .HasForeignKey(x => x.ClubId);*/
        }
    }
}
