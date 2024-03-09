using Crawler.Application.Common.Interfaces;
using Crawler.Application.Common.Settings;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Crawler.Application.Crawler.Queries.GetWordsAndImagesFromPage;

public class GetWordsAndImagesFromPageQueryHandler(ICrawlingService crawlingService, ILogger<GetWordsAndImagesFromPageQueryHandler> logger, IOptions<CrawlerSettings> options) 
    : IRequestHandler<GetWordsAndImagesFromPageQuery, ErrorOr<GetWordsAndImagesFromPageQueryResponse>>
{
    private readonly int _numberOfTopWords = options.Value.CountOfTopWordsThatWillBeReturned;

    public async Task<ErrorOr<GetWordsAndImagesFromPageQueryResponse>> Handle(GetWordsAndImagesFromPageQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to get all the images and top {count} words from the URL: {url}.", _numberOfTopWords, query.Url);

        var crawlingResult = await crawlingService.CrawlAsync(query.Url, cancellationToken);

        logger.LogInformation("Images and words received successfully from the URL: {url}.", query.Url);

        return new GetWordsAndImagesFromPageQueryResponse()
        {
            Url = query.Url,
            TopWords = GetTopWords(crawlingResult.Words),
            PageWordsCount = crawlingResult.WordsCount,
            Images = crawlingResult.ImageUrls
        };
    }

    private Dictionary<string, int> GetTopWords(IEnumerable<string> words)
    {
        var wordWithCountDictionary = new Dictionary<string, int>();

        if (!words.Any())
            return wordWithCountDictionary;

        foreach (var word in words)
        {
            if (wordWithCountDictionary.TryGetValue(word, out int value))
            {
                wordWithCountDictionary[word] = ++value;
                continue;
            }

            wordWithCountDictionary[word] = 1;
        }

        var topWords = wordWithCountDictionary
            .OrderByDescending(x => x.Value)
            .Take(_numberOfTopWords)
            .ToDictionary(x => x.Key, x => x.Value);

        return topWords;
    }
}
