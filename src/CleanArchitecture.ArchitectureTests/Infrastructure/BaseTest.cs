using System.Reflection;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Infrastructure;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CleanArchitecture.ArchitectureTests.Infrastructure;

public class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(IBaseCommand).Assembly;
    protected static readonly Assembly DomainAssembly = typeof(IEntity).Assembly;

    protected static readonly Assembly InfrastructureAssembly =
        typeof(ApplicationDbContext).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
