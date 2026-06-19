using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Application.Lookup.Departments.DTOs;
using AddressBook.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AddressBook.Application.Lookup.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
{
    private readonly IDepartmentRepository _repository;
    private readonly IMapper _mapper;

    public GetDepartmentByIdQueryHandler(IDepartmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DepartmentDto> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var department = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.Id);

        return _mapper.Map<DepartmentDto>(department);
    }
}
