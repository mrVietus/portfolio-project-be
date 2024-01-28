using ErrorOr;
using MediatR;

namespace Crawler.Application.Crawler.Queries.GetWordsAndImagesFromPage;

public record GetWordsAndImagesFromPageQuery(
    string Url
) : IRequest<ErrorOr<GetWordsAndImagesFromPageQueryResponse>>;
