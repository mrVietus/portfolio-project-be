using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Crawler.Application.Common.Behaviors;

[ExcludeFromCodeCoverage]
public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger) :
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.Log(LogLevel.Information, new EventId(0),
            "Started to command/query '{requestType}'. Request body: {requestBody}", request.GetType(), JsonConvert.SerializeObject(request));

        var response = await next();

        logger.Log(LogLevel.Information, new EventId(0),
            "Finished processing command/query '{requestType}'. Response body: {requestBody}", request.GetType(), JsonConvert.SerializeObject(response));

        return response;
    }
}
