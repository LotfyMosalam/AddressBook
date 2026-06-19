using MediatR;

namespace AddressBook.Application.AddressEntries.Commands.DeleteEntry;

public record DeleteEntryCommand(Guid Id) : IRequest;
