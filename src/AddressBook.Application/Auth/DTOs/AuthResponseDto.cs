namespace AddressBook.Application.Auth.DTOs;

public class AuthResponseDto
{
    public Guid UserId { get; init; }
    public string Token { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
