using AddressBook.Domain.Common;
using AddressBook.Domain.Enums;

namespace AddressBook.Domain.Entities;

public sealed class User : BaseEntity
{
    private User() { }

    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }

    public static User Create(string email, string passwordHash, UserRole role = UserRole.User)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash, nameof(passwordHash));

        return new User
        {
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            Role = role
        };
    }

    public void UpdatePasswordHash(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        PasswordHash = passwordHash;
    }

    public void ChangeRole(UserRole role) => Role = role;
}
