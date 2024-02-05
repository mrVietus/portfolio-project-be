using Crawler.Domain.Entities;

namespace Crawler.Application.Common.Interfaces.Repositories;

public interface ICrawlEfRepository : IRepository<CrawlEntity>
{
    Task<IEnumerable<CrawlEntity>> GetCrawlsForPageAsync(int page, int itemsPerPage);
    Task<CrawlEntity?> GetCrawlByIdWithCrawlResultAsync(Guid id);
}
