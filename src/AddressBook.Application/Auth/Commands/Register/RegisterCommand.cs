using AddressBook.Application.Auth.DTOs;
using MediatR;

namespace AddressBook.Application.Auth.Commands.Register;

public record RegisterCommand : IRequest<AuthResponseDto>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
