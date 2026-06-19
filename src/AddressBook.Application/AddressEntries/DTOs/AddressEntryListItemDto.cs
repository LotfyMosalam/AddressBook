namespace AddressBook.Application.AddressEntries.DTOs;

public class AddressEntryListItemDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string JobName { get; init; } = string.Empty;
    public string DepartmentName { get; init; } = string.Empty;
    public string MobileNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int Age { get; init; }
    public string? PhotoUrl { get; init; }
}
