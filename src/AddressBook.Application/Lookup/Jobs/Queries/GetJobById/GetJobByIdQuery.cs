using AddressBook.Application.Lookup.Jobs.DTOs;
using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Queries.GetJobById;

public record GetJobByIdQuery(Guid Id) : IRequest<JobDto>;
