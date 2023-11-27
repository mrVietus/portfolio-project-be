using System.Diagnostics.CodeAnalysis;
using Domain.Messages;
using FluentValidation;

namespace Application.Crawler.Queries;

[ExcludeFromCodeCoverage]
public class GetWordsAndImagesFromPageQueryValidator : AbstractValidator<GetWordsAndImagesFromPageQuery>
{
    private readonly string ProperUrlExpression = @"\b(?:https?://|www\.)\S+\b";

    public GetWordsAndImagesFromPageQueryValidator()
    {
        RuleFor(x => x.Url)
            .NotNull()
            .NotEmpty()
            .Matches(ProperUrlExpression)
                .WithMessage(Messages.InvalidUrlFormat);
    }
}
