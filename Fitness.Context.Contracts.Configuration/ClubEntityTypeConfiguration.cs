using Fitness.Context.Contracts.Configuration;
using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitness.Context.Contracts
{
    public class ClubEntityTypeConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
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
                .HasForeignKey(x => x.ClubId);
        }
    }
}
