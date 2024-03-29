﻿using Crawler.Application.Common.Interfaces;
using Crawler.Application.Common.Interfaces.Repositories;
using Crawler.Infrastructure.Cache;
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
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<ICacheService, MemoryCacheService>();

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
