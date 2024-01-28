using System.IO;
using Crawler.Application;
using Crawler.FunctionHandler.Mappings;
using Crawler.FunctionHandler.Middleware;
using Crawler.Infrastructure;
using Crawler.Infrastructure.Persistance.Database.Migrations.HostExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        workerApplication.UseMiddleware<ResponseCorsMiddleware>();
    })
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