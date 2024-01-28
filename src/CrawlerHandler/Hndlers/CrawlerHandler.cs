using System;
using System.Threading;
using System.Threading.Tasks;
using Crawler.Application.Common;
using Crawler.Application.Common.Interfaces;
using Crawler.Application.Crawler.Commands.RemoveCrawl;
using Crawler.Application.Crawler.Commands.SaveCrawl;
using Crawler.Application.Crawler.Queries.GetCrawls;
using Crawler.Application.Crawler.Queries.GetWordsAndImagesFromPage;
using Crawler.FunctionHandler.Errors;
using Crawler.FunctionHandler.Models;
using MapsterMapper;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Crawler.FunctionHandler.Hndlers;

internal class CrawlerHandler : BaseHandler
{
    private readonly ICacheService _cacheService;
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly ILogger<CrawlerHandler> _logger;

    public CrawlerHandler(ICacheService cacheService, ISender sender, IMapper mapper, ILogger<CrawlerHandler> logger)
    {
        _cacheService = cacheService;
        _sender = sender;
        _mapper = mapper;
        _logger = logger;
    }

    [Function(Constants.CrawlPageFunctionName)]
    public async Task<HttpResponseData> CrawlAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.CrawlPageFunctionRoute)]
        HttpRequestData request, string url, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning("CrawlAsync: No URL provided in query parameters.");
            return await ProblemAsync(request, ApiErrors.MissingUrlQueryParameter, cancellationToken);
        }

        var cachedValue = _cacheService.GetFromCache<GetWordsAndImagesFromPageQueryResponse>(url);
        if (cachedValue != null)
        {
            _logger.LogInformation("Got response from CACHE for CrawlAsync - url: {url}.", url);
            return await OkAsync(request, cachedValue, cancellationToken);
        }

        var query = new GetWordsAndImagesFromPageQuery(url);
        var queryResult = await _sender.Send(query, cancellationToken);

        return await queryResult.MatchAsync(
            async result => await CacheResponseAndRespondOkAsync(url, result, request, cancellationToken),
            async errors => await ProblemAsync(request, errors, cancellationToken)
        );
    }

    [Function(Constants.GetCrawlsFunctionName)]
    public async Task<HttpResponseData> GetCrawlsAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.GetCrawlsFunctionRoute)]
        HttpRequestData request, int pageNumber, int itemsPerPage, CancellationToken cancellationToken)
    {
        var query = new GetCrawlsQuery(pageNumber, itemsPerPage);
        var queryResult = await _sender.Send(query, cancellationToken);

        return queryResult.IsError ?
            await ProblemAsync(request, queryResult.Errors, cancellationToken) :
            await OkAsync(request, queryResult.Value, cancellationToken);
    }

    [Function(Constants.CreateCrawlFunctionName)]
    public async Task<HttpResponseData> CreateCrawlAsync(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = Constants.CreateCrawlFunctionRoute)]
        HttpRequestData request, CancellationToken cancellationToken)
    {
        var requestData = TryDeserializeRequestBody<SaveCrawlRequest>(request.Body);
        if (requestData is null)
        {
            _logger.LogWarning("CreateCrawlAsync received wrong object in body. Not able to deserialize.");
            return await ProblemAsync(request, ApiErrors.WrongBody, cancellationToken);
        }

        var command = _mapper.Map<SaveCrawlCommand>(requestData);
        var commandResult = await _sender.Send(command, cancellationToken);

        return commandResult.IsError ?
            await ProblemAsync(request, commandResult.Errors, cancellationToken) :
            await OkAsync(request, commandResult.Value, cancellationToken);
    }

    [Function(Constants.DeleteCrawlFunctionName)]
    public async Task<HttpResponseData> DeleteCrawlsAsync(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = Constants.DeleteCrawlFunctionRoute)]
        HttpRequestData request, Guid id, CancellationToken cancellationToken)
    {
        var command = new RemoveCrawlCommand(id);
        var commandResult = await _sender.Send(command, cancellationToken);

        return commandResult.IsError ?
            await ProblemAsync(request, commandResult.Errors, cancellationToken) :
            await OkAsync(request, commandResult.Value, cancellationToken);
    }

    public async Task<HttpResponseData> CacheResponseAndRespondOkAsync<T>(string key, T value, HttpRequestData request, CancellationToken cancellationToken)
    {
        _cacheService.SetCache(key, value);
        return await OkAsync(request, value, cancellationToken);
    }
}
