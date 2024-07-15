using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.UnitTests.Infrastructure;

/// <summary>
/// Clase base abstracta para pruebas unitarias, proporcionando métodos utilitarios
/// para verificar la publicación de eventos de dominio.
/// </summary>
public abstract class BaseTests
{
    public static T AssertDomainEventWasPublished<T>(IEntity entity)
        where T : IDomainEvent
    {
        var domainEvent = entity.GetDomainEvents().OfType<T>().SingleOrDefault();

        if (domainEvent is null)
        {
            throw new Exception($"{typeof(T).Name} no fue publicado");
        }
        return domainEvent!;
    }
}
