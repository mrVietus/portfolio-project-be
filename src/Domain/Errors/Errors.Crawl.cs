using ErrorOr;


namespace Crawler.Domain.Errors;

public static partial class Errors
{
    public static class Crawl
    {
        public static Error CrawlNotFound => Error.NotFound(
            code: "Crawl.CrawlNotFound",
            description: "Crawl was not found.");

        public static Error CrawlAlreadyExists => Error.Conflict(
            code: "Crawl.CrawlAlreadyExists",
            description: "Crawl with same Name already exists.");
    }
}
