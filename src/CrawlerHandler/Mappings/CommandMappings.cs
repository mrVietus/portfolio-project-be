using Crawler.Application.Crawler.Commands.SaveCrawl;
using Crawler.FunctionHandler.Models;
using Mapster;

namespace Crawler.FunctionHandler.Mappings;

public class CommandMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SaveCrawlRequest, SaveCrawlCommand>();
    }
}
