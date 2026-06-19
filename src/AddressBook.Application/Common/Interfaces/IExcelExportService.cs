using AddressBook.Application.AddressEntries.DTOs;

namespace AddressBook.Application.Common.Interfaces;

public interface IExcelExportService
{
    byte[] ExportAddressEntries(IReadOnlyList<AddressEntryListItemDto> entries);
}
