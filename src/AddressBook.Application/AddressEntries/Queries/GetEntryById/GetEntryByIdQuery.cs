using AddressBook.Application.AddressEntries.DTOs;
using MediatR;

namespace AddressBook.Application.AddressEntries.Queries.GetEntryById;

public record GetEntryByIdQuery(Guid Id) : IRequest<AddressEntryDto>;
