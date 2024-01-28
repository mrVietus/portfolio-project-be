using System.Diagnostics.CodeAnalysis;

namespace Crawler.Application.Common;

[ExcludeFromCodeCoverage]
public static class Constants
{
    public const string CrawlPageFunctionName = "CrawlPage";
    public const string CrawlPageFunctionRoute = "crawl/new";

    public const string GetCrawlsFunctionName = "GetCrawl";
    public const string GetCrawlsFunctionRoute = "crawls/page/{pageNumber}/itemsperpage/{itemsPerPage}";

    public const string CreateCrawlFunctionName = "SaveCrawl";
    public const string CreateCrawlFunctionRoute = "crawl";

    public const string DeleteCrawlFunctionName = "DeleteCrawl";
    public const string DeleteCrawlFunctionRoute = "crawl/{id}";
}
