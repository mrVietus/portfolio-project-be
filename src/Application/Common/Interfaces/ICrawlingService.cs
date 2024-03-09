using Crawler.Application.Models;

namespace Crawler.Application.Common.Interfaces;

public interface ICrawlingService
{
    Task<CrawlResult> CrawlAsync(string url, CancellationToken cancellationToken = default);
}
