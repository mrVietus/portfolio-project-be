using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crawler.Infrastructure.Persistance.Database.Migrations.HostExtensions;

public static class MigrationHostExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        using var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        appDbContext.Database.Migrate();

        return host;
    }
}
