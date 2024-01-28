using Crawler.Domain.Entities.Base;

namespace Crawler.Domain.Entities;

public class CrawlEntity : AuditData
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public CrawlResultEntity CrawlResult { get; set; }
    public Guid CrawlResultId { get; set; }

    public CrawlEntity() { }

    public CrawlEntity(string name, CrawlResultEntity crawlResult)
    {
        Id = Guid.NewGuid();

        CrawlResult = crawlResult;
        CrawlResultId = crawlResult.Id;

        Name = name;   
    }
}
