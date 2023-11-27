using Domain.Models;

namespace Application.Interfaces;

public interface ICrawlingService
{
    Task<CrawlResult> CrawlAsync(string url, CancellationToken cancellationToken = default);
}
