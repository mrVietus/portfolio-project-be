namespace Crawler.Application.Models;

public sealed class Crawl
{
    public required Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public IEnumerable<string> Images { get; init; } = [];
    public IDictionary<string, int> TopWords { get; init; } = new Dictionary<string, int>();
    public int PageWordsCount { get; init; }
    public DateTime CapturedAt { get; init; }
}
