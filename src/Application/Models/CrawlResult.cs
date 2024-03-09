namespace Crawler.Application.Models;

public sealed class CrawlResult
{
    public IEnumerable<string> ImageUrls { get; init; } = [];
    public IEnumerable<string> Words { get; init; } = [];
    public int WordsCount => Words.Count();
}
