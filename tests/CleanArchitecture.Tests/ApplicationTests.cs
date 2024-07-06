using FluentAssertions;
using NetArchTest.Rules;

namespace Crawler.CleanArchitecture.Tests;

[TestFixture]
public class ApplicationTests
{
    [Test]
    public void ApplicationShouldNotHaveDependencyOnInfrastructureAndFunctionHandler()
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
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void ApplicationShouldHaveDependencyOnDomain()
    {
        // Arrange
        var assembly = typeof(Application.Common.Constants).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .Should()
            .HaveDependencyOn("Domain")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
