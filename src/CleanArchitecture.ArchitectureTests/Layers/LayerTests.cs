using CleanArchitecture.ArchitectureTests.Infrastructure;
using FluentAssertions;
using NetArchTest.Rules;

namespace CleanArchitecture.ArchitectureTests.Layers;

public class LayerTests : BaseTest
{
    [Fact]
    public void DomainLayer_ShouldHaveNotDependency_ApplicationLayer()
    {
        var resultados = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        resultados.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_ShouldHaveNotDependency_InfrastructureLayer()
    {
        var resultados = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        resultados.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldHaveNotDependency_InfrastructureLayer()
    {
        var resultados = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        resultados.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldHaveNotDependency_PresentationLayer()
    {
        var resultados = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        resultados.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void InfrastructureLayer_ShouldHaveNotDependency_PresentationLayer()
    {
        var resultados = Types
            .InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        resultados.IsSuccessful.Should().BeTrue();
    }
}
