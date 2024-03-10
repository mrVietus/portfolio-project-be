using System;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Crawler.Application.Common;
using Crawler.Application.Common.Interfaces;
using Crawler.Application.Crawler.Commands.RemoveCrawl;
using Crawler.Application.Crawler.Commands.SaveCrawl;
using Crawler.Application.Crawler.Queries.GetCrawlById;
using Crawler.Application.Crawler.Queries.GetCrawls;
using Crawler.Application.Crawler.Queries.GetWordsAndImagesFromPage;
using Crawler.Application.Models;
using Crawler.FunctionHandler.Errors;
using Crawler.FunctionHandler.Models;
using MapsterMapper;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Crawler.FunctionHandler.Hndlers;

internal class CrawlerHandler(ICacheService cacheService, ISender sender, IMapper mapper, ILogger<CrawlerHandler> logger) : BaseHandler
{
    [Function(Constants.CrawlPageFunctionName)]
    [OpenApiOperation(operationId: Constants.CrawlPageFunctionName)]
    [OpenApiParameter(
        "url",
        Type = typeof(string),
        Required = true,
        Description = "Url of of page that user wants to crawl."
    )]
    [OpenApiResponseWithBody(
        HttpStatusCode.OK,
        MediaTypeNames.Application.Json,
        typeof(GetWordsAndImagesFromPageQueryResponse),
        Description = "Crawled page data."
    )]
    public async Task<HttpResponseData> CrawlAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.CrawlPageFunctionRoute)]
        HttpRequestData request, string url, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            logger.LogWarning("CrawlAsync: No URL provided in query parameters.");
            return await ProblemAsync(request, ApiErrors.MissingUrlQueryParameter, cancellationToken);
        }

        var cachedValue = cacheService.GetFromCache<GetWordsAndImagesFromPageQueryResponse>(url);
        if (cachedValue != null)
        {
            logger.LogInformation("Got response from CACHE for CrawlAsync - url: {url}.", url);
            return await OkAsync(request, cachedValue, cancellationToken);
        }

        var query = new GetWordsAndImagesFromPageQuery(url);
        var queryResult = await sender.Send(query, cancellationToken);

        return await queryResult.MatchAsync(
            async result => await CacheResponseAndRespondOkAsync(url, result, request, cancellationToken),
            async errors => await ProblemAsync(request, errors, cancellationToken)
        );
    }

    [Function("GetCrawlById")]
    [OpenApiOperation(operationId: "GetCrawlById")]
    [OpenApiParameter(
       "id",
       Type = typeof(string),
       Required = true,
       Description = "Guid id passed as a string."
   )]
    [OpenApiResponseWithBody(
       HttpStatusCode.OK,
       MediaTypeNames.Application.Json,
       typeof(GetCrawlsQueryResponse),
       Description = "Return crawl from db by id."
   )]
    public async Task<HttpResponseData> GetCrawlByIdAsync(
       [HttpTrigger(AuthorizationLevel.Function, "get", Route = "crawl/{id}")]
        HttpRequestData request, string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out Guid crawlId))
        {
            logger.LogWarning("GetCrawlByIdAsync received wrong id in parameter. Not able to parse to Guid.");
            return await ProblemAsync(request, ApiErrors.WrongDataProvided, cancellationToken);
        }

        var cachedValue = cacheService.GetFromCache<Crawl>(id);
        if (cachedValue != null)
        {
            logger.LogInformation("Got response from CACHE for GetCrawlByIdAsync - Id: {id}.", id);
            return await OkAsync(request, cachedValue, cancellationToken);
        }

        var query = new GetCrawlByIdQuery(crawlId);
        var queryResult = await sender.Send(query, cancellationToken);

        return queryResult.IsError ?
            await ProblemAsync(request, queryResult.Errors, cancellationToken) :
            await CacheResponseAndRespondOkAsync(id, queryResult.Value, request, cancellationToken);
    }

    [Function(Constants.GetCrawlsFunctionName)]
    [OpenApiOperation(operationId: Constants.GetCrawlsFunctionName)]
    [OpenApiParameter(
        "pageNumber",
        Type = typeof(int),
        Required = true,
        Description = "Number of page in pagination."
    )]
    [OpenApiParameter(
        "itemsPerPage",
        Type = typeof(int),
        Required = true,
        Description = "Number of items per page that we return in pagination."
    )]
    [OpenApiResponseWithBody(
        HttpStatusCode.OK,
        MediaTypeNames.Application.Json,
        typeof(GetCrawlsQueryResponse),
        Description = "Return all the crawls from db paginated."
    )]
    public async Task<HttpResponseData> GetCrawlsAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.GetCrawlsFunctionRoute)]
        HttpRequestData request, string pageNumber, string itemsPerPage, CancellationToken cancellationToken)
    {
        if (!int.TryParse(pageNumber, out int page))
        {
            logger.LogWarning("GetCrawlsAsync received wrong pageNumber in parameter. Not able to parse to int.");
            return await ProblemAsync(request, ApiErrors.WrongDataProvided, cancellationToken);
        }

        if (!int.TryParse(itemsPerPage, out int pageItemsCount))
        {
            logger.LogWarning("GetCrawlsAsync received wrong itemsPerPage in parameter. Not able to parse to int.");
            return await ProblemAsync(request, ApiErrors.WrongDataProvided, cancellationToken);
        }

        var query = new GetCrawlsQuery(page, pageItemsCount);
        var queryResult = await sender.Send(query, cancellationToken);

        return queryResult.IsError ?
            await ProblemAsync(request, queryResult.Errors, cancellationToken) :
            await OkAsync(request, queryResult.Value, cancellationToken);
    }

    [Function(Constants.CreateCrawlFunctionName)]
    [OpenApiOperation(operationId: Constants.CreateCrawlFunctionName)]
    [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(SaveCrawlRequest))]
    [OpenApiResponseWithBody(
        HttpStatusCode.Created,
        MediaTypeNames.Application.Json,
        typeof(Crawl),
        Description = "Return new crawl that was created into crawls database."
    )]
    public async Task<HttpResponseData> CreateCrawlAsync(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = Constants.CreateCrawlFunctionRoute)]
        HttpRequestData request, CancellationToken cancellationToken)
    {
        var requestData = TryDeserializeRequestBody<SaveCrawlRequest>(request.Body);
        if (requestData is null)
        {
            logger.LogWarning("CreateCrawlAsync received wrong object in body. Not able to deserialize.");
            return await ProblemAsync(request, ApiErrors.WrongBody, cancellationToken);
        }

        var command = mapper.Map<SaveCrawlCommand>(requestData);
        var commandResult = await sender.Send(command, cancellationToken);

        return commandResult.IsError ?
            await ProblemAsync(request, commandResult.Errors, cancellationToken) :
            await CreatedAsync(request, commandResult.Value, cancellationToken);
    }

    [Function(Constants.DeleteCrawlFunctionName)]
    [OpenApiOperation(operationId: Constants.DeleteCrawlFunctionName)]
    [OpenApiParameter(
        "id",
        Type = typeof(Guid),
        Required = true,
        Description = "Id of existing Crawl that we want to remove from database."
    )]
    [OpenApiResponseWithBody(
        HttpStatusCode.OK,
        MediaTypeNames.Application.Json,
        typeof(bool),
        Description = "Returns true if Crawl with Id was properly removed from database."
    )]
    public async Task<HttpResponseData> DeleteCrawlsAsync(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = Constants.DeleteCrawlFunctionRoute)]
        HttpRequestData request, string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out Guid crawlId))
        {
            logger.LogWarning("DeleteCrawlsAsync received wrong id in parameter. Not able to parse to Guid.");
            return await ProblemAsync(request, ApiErrors.IdIsNotCorrectValue, cancellationToken);
        }

        var command = new RemoveCrawlCommand(crawlId);
        var commandResult = await sender.Send(command, cancellationToken);

        return commandResult.IsError ?
            await ProblemAsync(request, commandResult.Errors, cancellationToken) :
            await OkAsync(request, commandResult.Value, cancellationToken);
    }

    public async Task<HttpResponseData> CacheResponseAndRespondOkAsync<T>(string key, T value, HttpRequestData request, CancellationToken cancellationToken)
    {
        cacheService.SetCache(key, value);
        return await OkAsync(request, value, cancellationToken);
    }
}
