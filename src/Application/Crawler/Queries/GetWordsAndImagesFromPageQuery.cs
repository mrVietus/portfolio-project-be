using ErrorOr;
using MediatR;

namespace Crawler.Application.Crawler.Queries;

public record GetWordsAndImagesFromPageQuery(
    string Url
) : IRequest<ErrorOr<GetWordsAndImagesFromPageQueryResponse>>;
