namespace Crawler.Application.Models;

public sealed class Crawl
{
    public required Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public IEnumerable<string> Images { get; set; } = [];
    public IDictionary<string, int> TopWords { get; set; } = new Dictionary<string, int>();
    public int PageWordsCount { get; set; }
    public DateTime CapturedAt { get; set; }
}
