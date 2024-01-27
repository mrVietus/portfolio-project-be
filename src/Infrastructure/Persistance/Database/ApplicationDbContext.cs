using System.Diagnostics.CodeAnalysis;
using Crawler.Infrastructure.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Infrastructure.Persistance.Database;

[ExcludeFromCodeCoverage]
public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
