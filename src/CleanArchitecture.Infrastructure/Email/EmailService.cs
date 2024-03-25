using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Infrastructure;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Email recipient, string subject, string body)
    {
        //TODO: aqu� deber�a implementarse la l�gica de env�o de correo electr�nico.
        //Simulamos un env�o del correo.
        return Task.CompletedTask;
    }
}