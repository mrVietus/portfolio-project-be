using Crawler.Domain.Entities.Base;

namespace Crawler.Domain.Entities;

public sealed class CrawlEntity : Entity
{
    public string Name { get; init; }

    public CrawlResultEntity? CrawlResult { get; init; }
    public Guid? CrawlResultId { get; init; }

    private CrawlEntity() { }

    public CrawlEntity(Guid id, string name, CrawlResultEntity crawlResult, DateTime creationDate) 
        : base(id, creationDate)
    {
        Name = name;
        CrawlResult = crawlResult;
        CrawlResultId = crawlResult.Id;
    }
}
