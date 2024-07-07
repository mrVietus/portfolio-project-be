namespace Crawler.CleanArchitecture.Tests;

[TestFixture]
public class DomainTests
{
    [Test]
    public void DomainShouldNotHaveDependencyOnOthers()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.CrawlEntity).Assembly;

        var otherProjects = new[]
        {
            "Crawler.Application",
            "Crawler.Infrastructure",
            "Crawler.FunctionHandler"
        };

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
