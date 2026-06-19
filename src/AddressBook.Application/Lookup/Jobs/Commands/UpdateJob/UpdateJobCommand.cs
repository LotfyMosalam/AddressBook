using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Commands.UpdateJob;

public record UpdateJobCommand(Guid Id, string Name) : IRequest;
