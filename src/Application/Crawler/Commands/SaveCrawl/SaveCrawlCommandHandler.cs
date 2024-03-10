using Crawler.Application.Common.Interfaces.Repositories;
using Crawler.Application.Models;
using Crawler.Domain.Entities;
using Crawler.Domain.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crawler.Application.Crawler.Commands.SaveCrawl;

internal class SaveCrawlCommandHandler(ICrawlEfRepository crawlEfRepository, IMapper mapper, TimeProvider timeProvider, ILogger<SaveCrawlCommandHandler> logger) 
    : IRequestHandler<SaveCrawlCommand, ErrorOr<Crawl>>
{
    public async Task<ErrorOr<Crawl>> Handle(SaveCrawlCommand command, CancellationToken cancellationToken)
    {
        var existingCrawlEntity = await crawlEfRepository.GetFirstOrDefaultAsync(c => c.Name == command.Name);
        if (existingCrawlEntity != null)
        {
            logger.LogWarning("Crawl with name:{name} already exists.", command.Name);
            return Errors.Crawl.CrawlAlreadyExists;
        }

        var creationDate = timeProvider.GetUtcNow().DateTime;

        var crawlResultEntity = new CrawlResultEntity(Guid.NewGuid(), command.Url, command.Images, command.TopWords, command.PageWordsCount, command.CapturedAt, creationDate);
        var crawlEntity = new CrawlEntity(Guid.NewGuid(), command.Name, crawlResultEntity, creationDate);

        await crawlEfRepository.InsertAsync(crawlEntity);

        var commandResponse = mapper.Map<Crawl>(crawlEntity);
        return commandResponse;
    }
}
