using System.IO;
using Crawler.Application;
using Crawler.FunctionHandler.Mappings;
using Crawler.Infrastructure;
using Crawler.Infrastructure.Persistance.Database.Migrations.HostExtensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(
        (context, builder) =>
        {
            var environment = context.HostingEnvironment.IsProduction()
                ? string.Empty
                : $"[{context.HostingEnvironment.EnvironmentName}]";

            builder.Services.AddSingleton<IOpenApiConfigurationOptions>(
                new OpenApiConfigurationOptions
                {
                    Info = new() { Version = "v1", Title = environment, },
                    Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                    OpenApiVersion = OpenApiVersionType.V3,
                    IncludeRequestingHostName = true,
                    ForceHttps = !context.HostingEnvironment.IsDevelopment(),
                    ForceHttp = false
                }
            );
        }
    )
    .ConfigureOpenApi()
    .ConfigureServices(
        (context, services) =>
        {
            services.AddApplication(context.Configuration);
            services.AddInfrastructure(context.Configuration);
            services.AddMemoryCache(); // Added only for demo purposes - normally we should add distributed cache for AzureFunction
            services.AddMappings();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddFilter(level => true);
            });
        }
    )
    .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
    {
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configurationBuilder.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        configurationBuilder.AddEnvironmentVariables();
        configurationBuilder.AddUserSecrets<Program>();
    })
    .Build();

host.MigrateDatabase();
host.Run();