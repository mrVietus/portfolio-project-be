namespace Crawler.Domain.Models;

public class Crawl
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public IEnumerable<string> Images { get; set; } = Enumerable.Empty<string>();
    public IDictionary<string, int> TopWords { get; set; } = new Dictionary<string, int>();
    public int PageWordsCount { get; set; }
    public DateTime CapturedAt { get; set;}
}
