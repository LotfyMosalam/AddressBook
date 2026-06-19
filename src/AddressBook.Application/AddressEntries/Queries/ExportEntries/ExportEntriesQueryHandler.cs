using AddressBook.Application.Common.Interfaces;
using MediatR;

namespace AddressBook.Application.AddressEntries.Queries.ExportEntries;

public class ExportEntriesQueryHandler : IRequestHandler<ExportEntriesQuery, byte[]>
{
    private readonly IAddressEntryRepository _repository;
    private readonly IExcelExportService _excelExportService;

    public ExportEntriesQueryHandler(
        IAddressEntryRepository repository,
        IExcelExportService excelExportService)
    {
        _repository = repository;
        _excelExportService = excelExportService;
    }

    public async Task<byte[]> Handle(ExportEntriesQuery request, CancellationToken cancellationToken)
    {
        var entries = await _repository.GetAllForExportAsync(cancellationToken);
        return _excelExportService.ExportAddressEntries(entries);
    }
}
