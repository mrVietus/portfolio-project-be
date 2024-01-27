using System.Threading;
using System.Threading.Tasks;
using Crawler.Application.Common.Interfaces;
using Crawler.Application.Crawler.Queries;
using Crawler.FunctionHandler.Errors;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Crawler.FunctionHandler.Hndlers;

internal class CrawlerHandler : BaseHandler
{
    private readonly ICacheService _cacheService;
    private readonly ISender _sender;
    private readonly ILogger<CrawlerHandler> _logger;

    public CrawlerHandler(ICacheService cacheService, ISender sender, ILogger<CrawlerHandler> logger)
    {
        _cacheService = cacheService;
        _sender = sender;
        _logger = logger;
    }

    [Function("GetImagesWithTopWords")]
    public async Task<HttpResponseData> GetImagesWithTopWordsAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "pagedata")]
        HttpRequestData request, string url, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning("No URL provided in query parameters.");
            return await ProblemAsync(request, ApiErrors.MissingUrlQueryParameter, cancellationToken);
        }

        var cachedValue = _cacheService.GetFromCache<GetWordsAndImagesFromPageQueryResponse>(url);
        if (cachedValue != null)
        {
            _logger.LogInformation("Got response from CACHE for url: {url}.", url);
            return await OkAsync(request, cachedValue, cancellationToken);
        }

        var query = new GetWordsAndImagesFromPageQuery(url);
        var queryResult = await _sender.Send(query, cancellationToken);

        return await queryResult.MatchAsync(
            async result => await CacheResponseAndRespondOkAsync(url, result, request, cancellationToken),
            async errors => await ProblemAsync(request, errors, cancellationToken)
        );
    }

    public async Task<HttpResponseData> CacheResponseAndRespondOkAsync<T>(string key, T value, HttpRequestData request, CancellationToken cancellationToken)
    {
        _cacheService.SetCache(key, value);
        return await OkAsync(request, value, cancellationToken);
    }
}
