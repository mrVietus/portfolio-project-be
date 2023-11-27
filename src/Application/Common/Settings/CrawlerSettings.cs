using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Settings;

[ExcludeFromCodeCoverage]
public class CrawlerSettings
{
    public const string SectionName = "Crawler";

    public string AllowedOrigin { get; init; } = string.Empty;
    public int CacheItemsTimeSpanInDays { get; init; } = 1;
    public int CountOfTopWordsThatWillBeReturned { get; init; } = 10;   
}
