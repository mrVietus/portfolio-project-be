using Crawler.Application.Common.Interfaces.Repositories;
using Crawler.Application.Models;
using Crawler.Domain.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;

namespace Crawler.Application.Crawler.Queries.GetCrawlById;

public class GetCrawlByIdQueryHandler(ICrawlEfRepository crawlEfRepository, IMapper mapper) 
    : IRequestHandler<GetCrawlByIdQuery, ErrorOr<Crawl>>
{
    public async Task<ErrorOr<Crawl>> Handle(GetCrawlByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await crawlEfRepository.GetCrawlByIdWithCrawlResultAsync(query.Id);
        if (result == null)
        {
            return Errors.Crawl.CrawlNotFound;
        }

        var queryResponse = mapper.Map<Crawl>(result);
        return queryResponse;
    }
}
