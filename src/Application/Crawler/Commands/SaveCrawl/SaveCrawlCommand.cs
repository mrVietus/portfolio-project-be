using Crawler.Application.Models;
using ErrorOr;
using MediatR;

namespace Crawler.Application.Crawler.Commands.SaveCrawl;

public record SaveCrawlCommand(
    string Name,
    string Url,
    IEnumerable<string> Images,
    IDictionary<string, int> TopWords,
    int PageWordsCount,
    DateTime CapturedAt
) : IRequest<ErrorOr<Crawl>>;
