using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Crawler.Application.Crawler.Commands.SaveCrawl;

[ExcludeFromCodeCoverage]
public class SaveCrawlCommandValidator : AbstractValidator<SaveCrawlCommand>
{
    public SaveCrawlCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(60);

        RuleFor(x => x.Url)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.PageWordsCount)
            .NotEmpty();

        RuleFor(x => x.CapturedAt)
            .NotNull()
            .NotEmpty();
    }
}
