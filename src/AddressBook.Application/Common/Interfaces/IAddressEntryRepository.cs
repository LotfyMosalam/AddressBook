using AddressBook.Application.AddressEntries.DTOs;
using AddressBook.Application.Common.Models;
using AddressBook.Domain.Entities;

namespace AddressBook.Application.Common.Interfaces;

public interface IAddressEntryRepository
{
    Task<AddressEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedResult<AddressEntryListItemDto>> GetPagedAsync(AddressEntryFilter filter, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AddressEntryListItemDto>> GetAllForExportAsync(CancellationToken cancellationToken = default);
    Task AddAsync(AddressEntry entry, CancellationToken cancellationToken = default);
    void Update(AddressEntry entry);
    void Remove(AddressEntry entry);
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
