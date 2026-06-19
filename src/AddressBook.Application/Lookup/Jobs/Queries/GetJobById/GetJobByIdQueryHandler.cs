using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Application.Lookup.Jobs.DTOs;
using AddressBook.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Queries.GetJobById;

public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, JobDto>
{
    private readonly IJobRepository _repository;
    private readonly IMapper _mapper;

    public GetJobByIdQueryHandler(IJobRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<JobDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Job), request.Id);

        return _mapper.Map<JobDto>(job);
    }
}
