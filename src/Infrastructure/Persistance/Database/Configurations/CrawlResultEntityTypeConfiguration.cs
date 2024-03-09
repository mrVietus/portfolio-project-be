using Crawler.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crawler.Infrastructure.Persistance.Database.Configurations;

public class CrawlResultEntityTypeConfiguration : IEntityTypeConfiguration<CrawlResultEntity>
{
    public void Configure(EntityTypeBuilder<CrawlResultEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .ValueGeneratedNever();

        builder
            .Property(e => e.Url)
            .IsRequired();

        builder
            .Property(e => e.Images);

        builder
            .Property(e => e.TopWordsJson);

        builder
            .Property(e => e.PageWordsCount)
            .IsRequired();

        builder
            .Property(e => e.CapturedAt)
            .IsRequired();

        builder
            .Property(e => e.Created)
            .IsRequired(true);

        builder
            .Property(e => e.Updated)
            .IsRequired(true)
            .HasDefaultValueSql("getdate()");

        builder
            .HasOne(e => e.Crawl)
            .WithOne(e => e.CrawlResult)
            .HasForeignKey<CrawlResultEntity>(e => e.CrawlId);
    }
}
