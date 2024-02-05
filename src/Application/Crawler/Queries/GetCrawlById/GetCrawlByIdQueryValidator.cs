using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Crawler.Application.Crawler.Queries.GetCrawlById;

[ExcludeFromCodeCoverage]
public class GetCrawlByIdQueryValidator : AbstractValidator<GetCrawlByIdQuery>
{
    public GetCrawlByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();
    }
}
