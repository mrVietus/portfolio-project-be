using Crawler.Application.Common.Interfaces.Repositories;
using Crawler.Domain.Entities;
using Crawler.Domain.Errors;
using Crawler.Domain.Models;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crawler.Application.Crawler.Commands.SaveCrawl;

internal class SaveCrawlCommandHandler : IRequestHandler<SaveCrawlCommand, ErrorOr<Crawl>>
{
    private readonly ICrawlEfRepository _crawlEfRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SaveCrawlCommandHandler> _logger;

    public SaveCrawlCommandHandler(ICrawlEfRepository crawlEfRepository, IMapper mapper, ILogger<SaveCrawlCommandHandler> logger)
    {
        _crawlEfRepository = crawlEfRepository;
        _mapper = mapper;
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

        var crawlResultEntity = new CrawlResultEntity(command.Url, command.Images, command.TopWords, command.PageWordsCount, command.CapturedAt);
        var crawlEntity = new CrawlEntity(command.Name, crawlResultEntity);

        await _crawlEfRepository.InsertAsync(crawlEntity);
        await _crawlEfRepository.SaveAsync();

        var commandResponse = _mapper.Map<Crawl>(crawlEntity);
        return commandResponse;
    }
}
