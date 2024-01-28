using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Crawler.Application.Crawler.Commands.RemoveCrawl;

[ExcludeFromCodeCoverage]
public class RemoveCrawlValidator : AbstractValidator<RemoveCrawlCommand>
{
    public RemoveCrawlValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();
    }
}
