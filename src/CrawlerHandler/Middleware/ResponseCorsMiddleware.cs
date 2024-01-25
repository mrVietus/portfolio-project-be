using System.Threading.Tasks;
using Crawler.Application.Common.Settings;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Options;

namespace Crawler.FunctionHandler.Middleware;

public class ResponseCorsMiddleware : IFunctionsWorkerMiddleware
{
    private readonly string _allowedOrigin;

    public ResponseCorsMiddleware(IOptions<CrawlerSettings> options)
    {
        _allowedOrigin = options.Value.AllowedOrigin;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        await next(context);

        context.GetHttpResponseData()?.Headers.Add("Access-Control-Allow-Origin", _allowedOrigin);
        context.GetHttpResponseData()?.Headers.Add("Access-Control-Allow-Methods", "GET");
        context.GetHttpResponseData()?.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
    }
}
