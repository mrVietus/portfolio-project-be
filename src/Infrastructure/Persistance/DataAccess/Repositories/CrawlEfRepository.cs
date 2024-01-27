using Crawler.Application.Common.Interfaces.Repositories;
using Crawler.Domain.Entities;
using Crawler.Infrastructure.Persistance.DataAccess.Repositories.Base;
using Crawler.Infrastructure.Persistance.Database;

namespace Crawler.Infrastructure.Persistance.DataAccess.Repositories;

public class CrawlEfRepository(ApplicationDbContext context) : EntityFrameworkRepository<Crawl>(context), ICrawlEfRepository
{
    public Task<IEnumerable<Crawl>> GetCrawlsForPageAsync(int page)
    {
        throw new NotImplementedException();
    }
}
