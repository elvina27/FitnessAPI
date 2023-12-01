using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fitness.Context.Contracts.Configuration;

namespace Fitness.Context.Contracts
{
    public class TimeTableItemEntityTypeConfiguration : IEntityTypeConfiguration<TimeTableItem>
    {
        public void Configure(EntityTypeBuilder<TimeTableItem> builder)
        {
            builder.ToTable("TimeTableItems");
            builder.HasIdAsKey();
            builder.PropertyAuditConfiguration();

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.StudyId)
                .IsRequired();

            builder.Property(x => x.GymId)
                .IsRequired();

            builder.Property(x => x.Warning)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasIndex(x => x.StartTime)
                .IsUnique()
                .HasFilter($"{nameof(TimeTableItem.DeletedAt)} is null")
                .HasDatabaseName($"IX_{nameof(TimeTableItem)}_{nameof(TimeTableItem.StartTime)}");
        }
    }
}
