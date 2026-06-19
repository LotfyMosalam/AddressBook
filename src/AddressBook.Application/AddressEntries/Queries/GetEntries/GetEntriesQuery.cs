using AddressBook.Application.AddressEntries.DTOs;
using AddressBook.Application.Common.Models;
using MediatR;

namespace AddressBook.Application.AddressEntries.Queries.GetEntries;

public record GetEntriesQuery : IRequest<PaginatedResult<AddressEntryListItemDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string SortBy { get; init; } = "FullName";
    public bool SortDescending { get; init; } = false;
}
