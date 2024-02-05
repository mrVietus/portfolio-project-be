using System.Diagnostics.CodeAnalysis;

namespace Crawler.Application.Common.Settings;

[ExcludeFromCodeCoverage]
public class CrawlerSettings
{
    public const string SectionName = "Crawler";

    public int CacheItemsTimeSpanInDays { get; init; } = 1;
    public int CountOfTopWordsThatWillBeReturned { get; init; } = 10;
}
