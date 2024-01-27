using Crawler.Domain.Entities;

namespace Crawler.Application.Common.Interfaces.Repositories;

public interface ICrawlEfRepository : IRepository<Crawl>
{
    Task<IEnumerable<Crawl>> GetCrawlsForPageAsync(int page);
}
