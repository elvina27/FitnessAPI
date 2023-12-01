using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fitness.Context.Contracts.Configuration;

namespace Fitness.Context.Contracts
{
    public class DocumentEntityTypeConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");
            builder.HasIdAsKey();
            builder.PropertyAuditConfiguration();
            builder.Property(x => x.Number)
               .HasMaxLength(8)
               .IsRequired();

            builder.Property(x => x.Series)
              .HasMaxLength(12)
              .IsRequired();

            builder.HasIndex(x => new { x.Number, x.Series })
                .IsUnique()
                .HasFilter($"{nameof(Document.DeletedAt)} is null")
                .HasDatabaseName($"IX_{nameof(Document)}_{nameof(Document.Number)}_{nameof(Document.Series)}");
        }
    }
}
