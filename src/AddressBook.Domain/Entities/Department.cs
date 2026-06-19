using AddressBook.Domain.Common;

namespace AddressBook.Domain.Entities;

public sealed class Department : BaseEntity
{
    private readonly List<AddressEntry> _addressEntries = [];

    private Department() { }

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<AddressEntry> AddressEntries => _addressEntries.AsReadOnly();

    public static Department Create(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        return new Department { Name = name.Trim() };
    }

    public void UpdateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Name = name.Trim();
    }
}
