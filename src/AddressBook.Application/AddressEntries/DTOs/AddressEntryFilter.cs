namespace AddressBook.Application.AddressEntries.DTOs;

public sealed class AddressEntryFilter
{
    // Global text search (FullName, Email, MobileNumber)
    public string? SearchTerm { get; init; }

    // Field-specific filters
    public string? FullName { get; init; }
    public string? Email { get; init; }
    public string? MobileNumber { get; init; }
    public string? Address { get; init; }

    // Lookup filters
    public Guid? JobId { get; init; }
    public Guid? DepartmentId { get; init; }

    // Date range
    public DateTime? DobFrom { get; init; }
    public DateTime? DobTo { get; init; }

    // Sorting
    public string SortBy { get; init; } = "FullName";
    public bool SortDescending { get; init; } = false;

    // Pagination
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
