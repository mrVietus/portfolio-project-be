using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Common.Behaviors;

[ExcludeFromCodeCoverage]
public class LoggingBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, new EventId(0),
            "Started to command/query '{requestType}'. Request body: {requestBody}", request.GetType(), JsonConvert.SerializeObject(request));

        var response = await next();

        _logger.Log(LogLevel.Information, new EventId(0),
            "Finished processing command/query '{requestType}'. Response body: {requestBody}", request.GetType(), JsonConvert.SerializeObject(response));

        return response;
    }
}
