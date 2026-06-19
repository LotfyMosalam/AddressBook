using AddressBook.Application.Auth.DTOs;
using MediatR;

namespace AddressBook.Application.Auth.Commands.Login;

public record LoginCommand : IRequest<AuthResponseDto>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
