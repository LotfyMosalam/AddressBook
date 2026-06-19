using AddressBook.Application.AddressEntries.DTOs;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Application.Common.Models;
using MediatR;

namespace AddressBook.Application.AddressEntries.Queries.GetEntries;

public class GetEntriesQueryHandler
    : IRequestHandler<GetEntriesQuery, PaginatedResult<AddressEntryListItemDto>>
{
    private readonly IAddressEntryRepository _repository;

    public GetEntriesQueryHandler(IAddressEntryRepository repository)
        => _repository = repository;

    public async Task<PaginatedResult<AddressEntryListItemDto>> Handle(
        GetEntriesQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new AddressEntryFilter
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDescending = request.SortDescending
        };

        return await _repository.GetPagedAsync(filter, cancellationToken);
    }
}
