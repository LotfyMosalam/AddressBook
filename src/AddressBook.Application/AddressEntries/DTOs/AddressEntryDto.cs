namespace AddressBook.Application.AddressEntries.DTOs;

public class AddressEntryDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public Guid JobId { get; init; }
    public string JobName { get; init; } = string.Empty;
    public Guid DepartmentId { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
    public string MobileNumber { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public int Age { get; init; }
    public string Address { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? PhotoUrl { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
