using FluentAssertions;
using NetArchTest.Rules;

namespace Crawler.CleanArchitecture.Tests;

[TestFixture]
public class InfrastructureTests
{
    [Test]
    public void InfrastructureShouldNotHaveDependencyOnDomainAndFunctionHandler()
    {
        // Arrange
        var assembly = typeof(Infrastructure.Common.Interfaces.IDbContext).Assembly;

        var otherProjects = new[]
        {
            "Domain",
            "FunctionHandler",
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
    public void InfrastructureShouldHaveDependencyOnAppliocation()
    {
        // Arrange
        var assembly = typeof(Infrastructure.Common.Interfaces.IDbContext).Assembly;

        // Act

        var result = Types.InAssembly(assembly)
            .Should()
            .HaveDependencyOnAny("Application")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
