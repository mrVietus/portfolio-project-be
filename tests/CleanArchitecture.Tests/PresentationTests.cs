using FluentAssertions;
using NetArchTest.Rules;

namespace Crawler.CleanArchitecture.Tests;

[TestFixture]
public class PresentationTests
{
    [Test]
    public void PresentationShouldNotHaveDependencyOnDomain()
    {
        // Arrange
        var assembly = typeof(FunctionHandler.Models.SaveCrawlRequest).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn("Domain")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void PresentationShouldHaveDependencyOnInfrastructureAndApplication()
    {
        // Arrange
        var assembly = typeof(Application.Common.Constants).Assembly;
        var otherProjects = new[]
        {
            "Application",
            "Infrastructure"
        };

        // Act
        var result = Types.InAssembly(assembly)
            .Should()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
