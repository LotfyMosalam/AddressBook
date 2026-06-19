using AddressBook.Application.AddressEntries.DTOs;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Application.Common.Models;
using MediatR;

namespace AddressBook.Application.AddressEntries.Queries.SearchEntries;

public class SearchEntriesQueryHandler
    : IRequestHandler<SearchEntriesQuery, PaginatedResult<AddressEntryListItemDto>>
{
    private readonly IAddressEntryRepository _repository;

    public SearchEntriesQueryHandler(IAddressEntryRepository repository)
        => _repository = repository;

    public async Task<PaginatedResult<AddressEntryListItemDto>> Handle(
        SearchEntriesQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new AddressEntryFilter
        {
            SearchTerm = request.SearchTerm,
            FullName = request.FullName,
            Email = request.Email,
            MobileNumber = request.MobileNumber,
            Address = request.Address,
            JobId = request.JobId,
            DepartmentId = request.DepartmentId,
            DobFrom = request.DobFrom,
            DobTo = request.DobTo,
            SortBy = request.SortBy,
            SortDescending = request.SortDescending,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return await _repository.GetPagedAsync(filter, cancellationToken);
    }
}
