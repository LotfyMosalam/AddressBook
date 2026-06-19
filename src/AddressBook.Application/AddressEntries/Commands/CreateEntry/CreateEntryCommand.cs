using MediatR;

namespace AddressBook.Application.AddressEntries.Commands.CreateEntry;

public record CreateEntryCommand : IRequest<Guid>
{
    public string FullName { get; init; } = string.Empty;
    public Guid JobId { get; init; }
    public Guid DepartmentId { get; init; }
    public string MobileNumber { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public string Address { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? PhotoUrl { get; init; }
}
