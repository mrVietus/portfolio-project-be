using Crawler.Application.Common.Interfaces.Repositories;
using ErrorOr;
using MapsterMapper;
using MediatR;

namespace Crawler.Application.Crawler.Queries.GetCrawls;

public class GetCrawlsQueryHandler(ICrawlEfRepository crawlEfRepository, IMapper mapper) 
    : IRequestHandler<GetCrawlsQuery, ErrorOr<GetCrawlsQueryResponse>>
{
    public async Task<ErrorOr<GetCrawlsQueryResponse>> Handle(GetCrawlsQuery query, CancellationToken cancellationToken)
    {
        var results = await crawlEfRepository.GetCrawlsForPageAsync(query.PageNumber, query.ItemsPerPage);
        if (!results.Any())
            return new GetCrawlsQueryResponse();

        var queryResponse = mapper.Map<GetCrawlsQueryResponse>(results);
        return queryResponse;
    }
}
