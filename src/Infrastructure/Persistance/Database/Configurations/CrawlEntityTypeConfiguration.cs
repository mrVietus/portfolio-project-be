using Crawler.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crawler.Infrastructure.Persistance.Database.Configurations;

public class CrawlEntityTypeConfiguration : IEntityTypeConfiguration<CrawlEntity>
{
    public void Configure(EntityTypeBuilder<CrawlEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .ValueGeneratedNever();

        builder
            .Property(e => e.Name)
            .HasMaxLength(60)
            .IsRequired();

        builder
            .Property(e => e.Created)
            .IsRequired(true)
            .HasDefaultValueSql("getdate()");

        builder
            .Property(e => e.Updated)
            .IsRequired(true)
            .HasDefaultValueSql("getdate()");

        builder
           .HasOne(e => e.CrawlResult)
           .WithOne(e => e.Crawl)
           .HasForeignKey<CrawlEntity>(e => e.CrawlResultId);
    }
}
