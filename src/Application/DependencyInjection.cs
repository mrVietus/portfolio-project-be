using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Crawler.Application.Common.Behaviors;
using Crawler.Application.Common.Interfaces;
using Crawler.Application.Common.Settings;
using Crawler.Application.Services;
using FluentValidation;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddTransient<ICrawlingService, CrawlingService>();
        services.AddTransient<ICacheService, MemoryCacheService>();
        services.AddTransient<HtmlWeb>();

        services.Configure<CrawlerSettings>(configuration.GetSection(CrawlerSettings.SectionName));

        return services;
    }
}
