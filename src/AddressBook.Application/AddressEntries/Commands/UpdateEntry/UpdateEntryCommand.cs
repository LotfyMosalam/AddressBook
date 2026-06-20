using MediatR;

namespace AddressBook.Application.AddressEntries.Commands.UpdateEntry;

public record UpdateEntryCommand : IRequest
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public Guid JobId { get; init; }
    public Guid DepartmentId { get; init; }
    public string MobileNumber { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public string Address { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
