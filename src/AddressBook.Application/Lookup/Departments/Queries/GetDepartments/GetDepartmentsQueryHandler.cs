using AddressBook.Application.Common.Interfaces;
using AddressBook.Application.Lookup.Departments.DTOs;
using AutoMapper;
using MediatR;

namespace AddressBook.Application.Lookup.Departments.Queries.GetDepartments;

public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, IReadOnlyList<DepartmentDto>>
{
    private readonly IDepartmentRepository _repository;
    private readonly IMapper _mapper;

    public GetDepartmentsQueryHandler(IDepartmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DepartmentDto>> Handle(
        GetDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var departments = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
    }
}
