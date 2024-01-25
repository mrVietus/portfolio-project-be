using Crawler.Domain.Models;

namespace Crawler.Application.Interfaces;

public interface ICrawlingService
{
    Task<CrawlResult> CrawlAsync(string url, CancellationToken cancellationToken = default);
}
