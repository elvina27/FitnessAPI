using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitness.Context.Contracts.Configuration
{
    public class StudyEntityTypeConfiguration : IEntityTypeConfiguration<Study>
    {
        void IEntityTypeConfiguration<Study>.Configure(EntityTypeBuilder<Study> builder)
        {
            builder.ToTable("Studyes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.PropertyAuditConfiguration();

            builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
            builder.HasIndex(x => x.Title)
                .IsUnique()
                .HasFilter($"{nameof(Study.DeletedAt)} is null")
                .HasDatabaseName($"IX_{nameof(Study)}_{nameof(Study.Title)}");
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Duration).HasMaxLength(3).IsRequired();
            builder.HasMany(x => x.TimeTableItems).WithOne(x => x.Study).HasForeignKey(x => x.StudyId);

            

            

            







            /*

            builder.ToTable("Studyes");
            builder.HasIdAsKey();
            builder.PropertyAuditConfiguration();
            builder.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .HasMany(x => x.TimeTableItems)
                .WithOne(x => x.Study)
                .HasForeignKey(x => x.StudyId);

            builder.HasIndex(x => x.Title)
                .IsUnique()
                .HasFilter($"{nameof(Study.DeletedAt)} is null")
                .HasDatabaseName($"IX_{nameof(Study)}_{nameof(Study.Title)}");

            builder.Property(x => x.Duration)
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);*/
        }
    }
}
