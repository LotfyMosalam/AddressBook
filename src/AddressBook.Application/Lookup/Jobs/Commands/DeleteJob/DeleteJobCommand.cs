using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Commands.DeleteJob;

public record DeleteJobCommand(Guid Id) : IRequest;
