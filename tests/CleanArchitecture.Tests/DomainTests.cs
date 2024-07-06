using FluentAssertions;
using NetArchTest.Rules;

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
            "Application",
            "Infrastructure",
            "FunctionHandler"
        };

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
