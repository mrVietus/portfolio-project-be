using ErrorOr;
using MediatR;

namespace Application.Crawler.Queries;

public record GetWordsAndImagesFromPageQuery(
    string Url
) : IRequest<ErrorOr<GetWordsAndImagesFromPageQueryResponse>>;
