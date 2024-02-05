using Crawler.Application.Common.Interfaces.Repositories;
using Crawler.Domain.Errors;
using Crawler.Domain.Models;
using ErrorOr;
using MapsterMapper;
using MediatR;

namespace Crawler.Application.Crawler.Queries.GetCrawlById;

public class GetCrawlByIdQueryHandler : IRequestHandler<GetCrawlByIdQuery, ErrorOr<Crawl>>
{
    private readonly ICrawlEfRepository _crawlEfRepository;
    private readonly IMapper _mapper;

    public GetCrawlByIdQueryHandler(ICrawlEfRepository crawlEfRepository, IMapper mapper)
    {
        _crawlEfRepository = crawlEfRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Crawl>> Handle(GetCrawlByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _crawlEfRepository.GetCrawlByIdWithCrawlResultAsync(query.Id);
        if (result == null)
        {
            return Errors.Crawl.CrawlNotFound;
        }

        var queryResponse = _mapper.Map<Crawl>(result);
        return queryResponse;
    }
}
