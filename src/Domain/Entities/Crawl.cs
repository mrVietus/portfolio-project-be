using Crawler.Domain.Entities.Base;

namespace Crawler.Domain.Entities;

public class Crawl : AuditData
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public CrawlResult CrawlResult { get; set; }
    public Guid CrawlResultId { get; set; }

    public Crawl() { }

    public Crawl(string name, CrawlResult crawlResult)
    {
        Id = Guid.NewGuid();

        CrawlResult = crawlResult;
        CrawlResultId = crawlResult.Id;

        Name = name;   
    }
}
