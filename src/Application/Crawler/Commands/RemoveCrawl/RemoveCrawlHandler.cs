using Crawler.Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Crawler.Domain.Errors;

namespace Crawler.Application.Crawler.Commands.RemoveCrawl;

public class RemoveCrawlHandler(ICrawlEfRepository crawlEfRepository, ILogger<RemoveCrawlHandler> logger) 
    : IRequestHandler<RemoveCrawlCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(RemoveCrawlCommand command, CancellationToken cancellationToken)
    {
        var crawlEntity = await crawlEfRepository.GetFirstOrDefaultAsync(c => c.Id == command.Id);
        if (crawlEntity is null)
        {
            logger.LogWarning("Crawl with Id: {Id} was not found.", command.Id);
            return Errors.Crawl.CrawlNotFound;
        }

        crawlEfRepository.Delete(crawlEntity);
        await crawlEfRepository.SaveAsync();

        logger.LogInformation("Crawl with Id: {Id} was found and removed.", command.Id);
        return true;
    }
}
