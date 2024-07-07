namespace Crawler.CleanArchitecture.Tests;

[TestFixture]
public class ApplicationTests
{
    [Test]
    public void ApplicationShouldNotHaveDependencyOnInfrastructureAndFunctionHandler()
    {
        // Arrange
        var assembly = typeof(Application.DependencyInjection).Assembly;

        var otherProjects = new[]
        {
            "Crawler.FunctionHandler",
            "Crawler.Infrastructure"
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
