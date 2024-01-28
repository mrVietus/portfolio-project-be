using Crawler.Application.Common.Interfaces.Repositories;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Crawler.Domain.Errors;

namespace Crawler.Application.Crawler.Commands.RemoveCrawl;

public class RemoveCrawlHandler : IRequestHandler<RemoveCrawlCommand, ErrorOr<bool>>
{
    private readonly ICrawlEfRepository _crawlEfRepository;
    private readonly ILogger<RemoveCrawlHandler> _logger;

    public RemoveCrawlHandler(ICrawlEfRepository crawlEfRepository, ILogger<RemoveCrawlHandler> logger)
    {
        _crawlEfRepository = crawlEfRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<bool>> Handle(RemoveCrawlCommand command, CancellationToken cancellationToken)
    {
        var crawlEntity = await _crawlEfRepository.GetFirstOrDefaultAsync(c => c.Id == command.Id);
        if (crawlEntity is null)
        {
            _logger.LogWarning("Crawl with Id: {Id} was not found.", command.Id);
            return Errors.Crawl.CrawlNotFound;
        }

        _crawlEfRepository.Delete(crawlEntity);
        await _crawlEfRepository.SaveAsync();

        _logger.LogInformation("Crawl with Id: {Id} was found and removed.", command.Id);
        return true;
    }
}
