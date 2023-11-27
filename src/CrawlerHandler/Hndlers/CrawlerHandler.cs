using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Crawler.Queries;
using Application.Interfaces;
using FunctionHandler.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionHandler.Hndlers;

internal class CrawlerHandler : BaseHandler
{
    private readonly ICacheService _cacheService;
    private readonly ISender _sender;
    private readonly ILogger<CrawlerHandler> _logger;

    private const string UrlQueryParam = "url";

    public CrawlerHandler(ICacheService cacheService, ISender sender, ILogger<CrawlerHandler> logger)
    {
        _cacheService = cacheService;
        _sender = sender;
        _logger = logger;
    }

    [FunctionName("GetImagesWithTopWords")]
    public async Task<IActionResult> GetImagesWithTopWordsAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "pagedata")]
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var queryParams = QueryHelpers.ParseNullableQuery(request.RequestUri.Query);
        if (queryParams == null || !queryParams.ContainsKey(UrlQueryParam))
        {
            _logger.LogWarning("No URL provided in query parameters.");
            return Problem(ApiErrors.MissingUrlQueryParameter);
        }

        var url = queryParams[UrlQueryParam];

        var cachedValue = _cacheService.GetFromCache<GetWordsAndImagesFromPageQueryResponse>(url);
        if (cachedValue != null)
        {
            _logger.LogInformation("Got response from CACHE for url: {url}.", url);
            return Ok(cachedValue);
        }

        var query = new GetWordsAndImagesFromPageQuery(url);
        var queryResult = await _sender.Send(query, cancellationToken);

        return queryResult.Match(
            result => CacheResponseAndRespondOk(url, result),
            errors => Problem(errors)
        );
    }

    public OkObjectResult CacheResponseAndRespondOk<T>(string key, T value)
    {
        _cacheService.SetCache<T>(key, value);
        return Ok(value);
    }
}
