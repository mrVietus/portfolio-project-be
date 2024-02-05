using Crawler.Application.Common.Interfaces.Repositories;
using Crawler.Domain.Entities;
using Crawler.Infrastructure.Persistance.DataAccess.Repositories.Base;
using Crawler.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Infrastructure.Persistance.DataAccess.Repositories;

public class CrawlEfRepository(ApplicationDbContext context) : EntityFrameworkRepository<CrawlEntity>(context), ICrawlEfRepository
{
    public async Task<IEnumerable<CrawlEntity>> GetCrawlsForPageAsync(int page, int itemsPerPage)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(page);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(itemsPerPage);

        int numberOfSkippedItems = 0;
        if (page > 1)
        {
            numberOfSkippedItems = page * itemsPerPage;
        }

        var query = DbSet
            .Include(x => x.CrawlResult)
            .Skip(numberOfSkippedItems)
            .Take(itemsPerPage)
            .OrderBy(x => x.Created)
            .AsQueryable();

        var crawlData = await query.ToListAsync();
        return crawlData;
    }

    public async Task<CrawlEntity?> GetCrawlByIdWithCrawlResultAsync(Guid id)
    {
        var query = DbSet
            .Include(x => x.CrawlResult)
            .Where(x => x.Id == id)
            .AsQueryable();

        var crawlEntity = await query.SingleOrDefaultAsync();
        return crawlEntity;
    }
}
