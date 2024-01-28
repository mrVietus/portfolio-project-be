using ErrorOr;
using MediatR;

namespace Crawler.Application.Crawler.Queries.GetCrawls;

public record GetCrawlsQuery(
    int PageNumber,
    int ItemsPerPage
) : IRequest<ErrorOr<GetCrawlsQueryResponse>>;
