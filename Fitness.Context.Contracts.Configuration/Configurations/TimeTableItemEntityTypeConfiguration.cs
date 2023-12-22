using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace Fitness.Context.Contracts.Configuration
{
    public class TimeTableItemEntityTypeConfiguration : IEntityTypeConfiguration<TimeTableItem>
    {
        void IEntityTypeConfiguration<TimeTableItem>.Configure(EntityTypeBuilder<TimeTableItem> builder)
        {
            builder.ToTable("TimeTableItems");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.PropertyAuditConfiguration();
            builder.Property(x => x.StartTime).IsRequired();
            builder.HasIndex(x => x.StartTime)
                .IsUnique()
                .HasFilter($"{nameof(TimeTableItem.DeletedAt)} is null")
                .HasDatabaseName($"IX_{nameof(TimeTableItem)}_{nameof(TimeTableItem.StartTime)}");
            builder.Property(x => x.StudyId).IsRequired();
            builder.Property(x => x.GymId).IsRequired();
            builder.Property(x => x.Warning).HasMaxLength(500).IsRequired();

            





            /*
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
                .HasDatabaseName($"IX_{nameof(TimeTableItem)}_{nameof(TimeTableItem.StartTime)}");*/
        }
    }
}
