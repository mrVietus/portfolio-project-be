namespace Crawler.CleanArchitecture.Tests;

public class InfrastructureTests
{
    [Test]
    public void InfrastructureShouldNotHaveDependencyOnPresentationLayer()
    {
        // Arrange
        var assembly = typeof(Infrastructure.DependencyInjection).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn("Crawler.FunctionHandler")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
