namespace Crawler.Application.Models;

public sealed class CrawlResult
{
    public IEnumerable<string> ImageUrls { get; set; } = [];
    public IEnumerable<string> Words { get; set; } = [];
    public int WordsCount => Words.Count();
}
