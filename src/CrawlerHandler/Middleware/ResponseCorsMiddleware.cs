using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Common.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunctionHandler.Middleware;

public class ResponseCorsMiddleware
{
    private readonly string _allowedOrigin;

    public ResponseCorsMiddleware(IOptions<CrawlerSettings> options)
    {
        _allowedOrigin = options.Value.AllowedOrigin;
    }

    public async Task<HttpResponseMessage> Run(HttpRequest req, ILogger log, ExecutionContext context, Func<HttpRequest, ILogger, Task<HttpResponseMessage>> next)
    {
        var response = await next(req, log);

        response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5173");
        response.Headers.Add("Access-Control-Allow-Methods", "GET");
        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

        return response;
    }
}
