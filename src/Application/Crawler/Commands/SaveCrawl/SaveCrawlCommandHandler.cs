using Crawler.Application.Common.Interfaces.Repositories;
using Crawler.Application.Models;
using Crawler.Domain.Entities;
using Crawler.Domain.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crawler.Application.Crawler.Commands.SaveCrawl;

internal class SaveCrawlCommandHandler : IRequestHandler<SaveCrawlCommand, ErrorOr<Crawl>>
{
    private readonly ICrawlEfRepository _crawlEfRepository;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<SaveCrawlCommandHandler> _logger;

    public SaveCrawlCommandHandler(ICrawlEfRepository crawlEfRepository, IMapper mapper, TimeProvider timeProvider, ILogger<SaveCrawlCommandHandler> logger)
    {
        _crawlEfRepository = crawlEfRepository;
        _mapper = mapper;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task<ErrorOr<Crawl>> Handle(SaveCrawlCommand command, CancellationToken cancellationToken)
    {
        var existingCrawlEntity = await _crawlEfRepository.GetFirstOrDefaultAsync(c => c.Name == command.Name);
        if (existingCrawlEntity != null)
        {
            _logger.LogWarning("Crawl with name:{name} already exists.", command.Name);
            return Errors.Crawl.CrawlAlreadyExists;
        }

        var creationDate = _timeProvider.GetUtcNow().DateTime;

        var crawlResultEntity = new CrawlResultEntity(Guid.NewGuid(), command.Url, command.Images, command.TopWords, command.PageWordsCount, command.CapturedAt, creationDate);
        var crawlEntity = new CrawlEntity(Guid.NewGuid(), command.Name, crawlResultEntity, creationDate);

        await _crawlEfRepository.InsertAsync(crawlEntity);
        await _crawlEfRepository.SaveAsync();

        var commandResponse = _mapper.Map<Crawl>(crawlEntity);
        return commandResponse;
    }
}
