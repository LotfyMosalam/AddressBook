using AddressBook.Application.Lookup.Jobs.DTOs;
using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Queries.GetJobs;

public record GetJobsQuery : IRequest<IReadOnlyList<JobDto>>;
