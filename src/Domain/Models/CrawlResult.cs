namespace Domain.Models;

public class CrawlResult
{
    public IEnumerable<string> ImageUrls { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<string> Words { get; set; } = Enumerable.Empty<string>();
    public int WordsCount => Words.Count(); 
}
