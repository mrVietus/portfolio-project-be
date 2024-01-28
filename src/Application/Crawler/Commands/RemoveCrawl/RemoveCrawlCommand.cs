using ErrorOr;
using MediatR;

namespace Crawler.Application.Crawler.Commands.RemoveCrawl;

public record RemoveCrawlCommand(
    Guid Id
) : IRequest<ErrorOr<bool>>;
