using Crawler.Domain.Models;

namespace Crawler.Application.Crawler.Queries.GetCrawls;

public class GetCrawlsQueryResponse
{
    public IEnumerable<Crawl> Crawls { get; set; } = Enumerable.Empty<Crawl>();
}
