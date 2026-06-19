using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Commands.CreateJob;

public record CreateJobCommand(string Name) : IRequest<Guid>;
