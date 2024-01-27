using System.Text.Json;
using Crawler.Domain.Entities.Base;

namespace Crawler.Domain.Entities;

public class CrawlResult : AuditData
{
    private readonly char _delimiter = ',';

    public Guid Id { get; private set; }
    public string Url { get; private set; }
    public string Images { get; private set; }
    public string TopWordsJson { get; private set; }
    public int PageWordsCount { get; private set; }
    public DateTime CapturedAt { get; private set; }

    public Crawl Crawl { get; set; }
    public Guid CrawlId { get; set; }

    public CrawlResult() { }

    public CrawlResult(string url, IEnumerable<string> images, IDictionary<string, int> topWords,
        int pageWordsCount, DateTime capturedAt)
    {
        Id = Guid.NewGuid();
        Url = url;
        Images = images.Any() ?
            string.Join(_delimiter, images) : string.Empty;
        TopWordsJson = JsonSerializer.Serialize(topWords);
        PageWordsCount = pageWordsCount;
        CapturedAt = capturedAt;
    }

    public IEnumerable<string> GetImagesAsList()
    {
        if (string.IsNullOrWhiteSpace(Images))
        {
            return Enumerable.Empty<string>();
        }

        return Images.Split(_delimiter)
            .ToList();
    }

    public IDictionary<string, int>? GetTopWordsAsDictionary()
    {
        if (string.IsNullOrWhiteSpace(TopWordsJson))
        {
            return new Dictionary<string, int>();
        }
        
        return JsonSerializer
            .Deserialize<Dictionary<string, int>>(TopWordsJson);
    }
}
