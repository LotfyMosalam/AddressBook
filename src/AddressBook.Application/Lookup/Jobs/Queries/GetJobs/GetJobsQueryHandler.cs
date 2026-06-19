using AddressBook.Application.Common.Interfaces;
using AddressBook.Application.Lookup.Jobs.DTOs;
using AutoMapper;
using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Queries.GetJobs;

public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, IReadOnlyList<JobDto>>
{
    private readonly IJobRepository _repository;
    private readonly IMapper _mapper;

    public GetJobsQueryHandler(IJobRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<JobDto>> Handle(
        GetJobsQuery request,
        CancellationToken cancellationToken)
    {
        var jobs = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<JobDto>>(jobs);
    }
}
