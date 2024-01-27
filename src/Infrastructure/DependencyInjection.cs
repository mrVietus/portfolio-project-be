using Crawler.Application.Common.Interfaces.Repositories;
using Crawler.Infrastructure.Persistance.DataAccess.Repositories;
using Crawler.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            configuration.GetConnectionString("CrawlerDatabase")));

        services.AddScoped<ICrawlEfRepository, CrawlEfRepository>();

        return services;
    }
}
