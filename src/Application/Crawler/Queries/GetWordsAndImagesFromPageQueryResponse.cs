namespace Crawler.Application.Crawler.Queries;

public class GetWordsAndImagesFromPageQueryResponse
{
    public string Url { get; set; } = string.Empty;
    public IEnumerable<string> Images { get; set; } = Enumerable.Empty<string>();
    public IDictionary<string, int> TopWords { get; set; } = new Dictionary<string, int>();
    public int PageWordsCount { get; set; }
    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;
}
