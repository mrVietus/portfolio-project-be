using System.Text.Json;
using Crawler.Domain.Entities.Base;

namespace Crawler.Domain.Entities;

public sealed class CrawlResultEntity : Entity
{
    private readonly char _delimiter = ',';

    public string Url { get; init; }
    public string Images { get; init; }
    public string TopWordsJson { get; init; }
    public int PageWordsCount { get; init; }
    public DateTime CapturedAt { get; init; }

    public CrawlEntity? Crawl { get; init; }
    public Guid? CrawlId { get; init; }

    private CrawlResultEntity() { }

    public CrawlResultEntity(Guid id, string url, IEnumerable<string> images, IDictionary<string, int> topWords,
        int pageWordsCount, DateTime capturedAt, DateTime creationDate) 
        : base(id, creationDate)
    {
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
            return [];
        }

        return [.. Images.Split(_delimiter)];
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
