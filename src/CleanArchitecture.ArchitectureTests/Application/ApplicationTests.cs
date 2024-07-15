using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.ArchitectureTests.Infrastructure;
using FluentAssertions;
using NetArchTest.Rules;

namespace CleanArchitecture.ArchitectureTests.Application;

public class ApplicationTests : BaseTest
{
    [Fact]
    public void CommandHandler_Should_NotBePublic()
    {
        var resultados = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .NotBePublic()
            .GetResult();

        resultados.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandler_Should_NotBePublic()
    {
        var resultados = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .NotBePublic()
            .GetResult();

        resultados.IsSuccessful.Should().BeTrue();
    }
}
