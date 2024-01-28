using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Crawler.Application.Crawler.Queries.GetCrawls;

[ExcludeFromCodeCoverage]
public class GetCrawlsQueryValidator : AbstractValidator<GetCrawlsQuery>
{
    public GetCrawlsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.ItemsPerPage)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
    }
}
