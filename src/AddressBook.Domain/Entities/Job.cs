using AddressBook.Domain.Common;

namespace AddressBook.Domain.Entities;

public sealed class Job : BaseEntity
{
    private readonly List<AddressEntry> _addressEntries = [];

    private Job() { }

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<AddressEntry> AddressEntries => _addressEntries.AsReadOnly();

    public static Job Create(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        return new Job { Name = name.Trim() };
    }

    public void UpdateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Name = name.Trim();
    }
}
