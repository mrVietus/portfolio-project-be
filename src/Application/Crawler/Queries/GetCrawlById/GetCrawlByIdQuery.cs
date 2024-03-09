using Crawler.Application.Models;
using ErrorOr;
using MediatR;

namespace Crawler.Application.Crawler.Queries.GetCrawlById;

public record GetCrawlByIdQuery(
    Guid Id
) : IRequest<ErrorOr<Crawl>>;
