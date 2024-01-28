using Crawler.Application.Common.Interfaces.Repositories;
using ErrorOr;
using MapsterMapper;
using MediatR;

namespace Crawler.Application.Crawler.Queries.GetCrawls;

public class GetCrawlsQueryHandler : IRequestHandler<GetCrawlsQuery, ErrorOr<GetCrawlsQueryResponse>>
{
    private readonly ICrawlEfRepository _crawlEfRepository;
    private readonly IMapper _mapper;

    public GetCrawlsQueryHandler(ICrawlEfRepository crawlEfRepository, IMapper mapper)
    {
        _crawlEfRepository = crawlEfRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<GetCrawlsQueryResponse>> Handle(GetCrawlsQuery query, CancellationToken cancellationToken)
    {
        var results = await _crawlEfRepository.GetCrawlsForPageAsync(query.PageNumber, query.ItemsPerPage);
        if (!results.Any())
            return new GetCrawlsQueryResponse();

        var queryResponse = _mapper.Map<GetCrawlsQueryResponse>(results);
        return queryResponse;
    }
}
