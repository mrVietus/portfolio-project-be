using System.Collections.Generic;
using Crawler.Application.Crawler.Queries.GetCrawls;
using Crawler.Domain.Entities;
using Crawler.Domain.Models;
using Mapster;

namespace Crawler.FunctionHandler.Mappings;

public class EntityMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CrawlEntity, Crawl>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Url, src => src.CrawlResult.Url)
            .Map(dest => dest.Images, src => src.CrawlResult.GetImagesAsList())
            .Map(dest => dest.TopWords, src => src.CrawlResult.GetTopWordsAsDictionary())
            .Map(dest => dest.PageWordsCount, src => src.CrawlResult.PageWordsCount)
            .Map(dest => dest.CapturedAt, src => src.CrawlResult.CapturedAt);

        config.NewConfig<IEnumerable<CrawlEntity>, GetCrawlsQueryResponse>()
            .Map(dest => dest.Crawls, src => src);
    }
}
