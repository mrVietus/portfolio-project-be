using Crawler.Application.Models;

namespace Crawler.Application.Crawler.Queries.GetCrawls;

public class GetCrawlsQueryResponse
{
    public IEnumerable<Crawl> Crawls { get; set; } = [];
}
