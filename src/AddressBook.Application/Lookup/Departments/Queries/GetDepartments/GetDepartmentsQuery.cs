using AddressBook.Application.Lookup.Departments.DTOs;
using MediatR;

namespace AddressBook.Application.Lookup.Departments.Queries.GetDepartments;

public record GetDepartmentsQuery : IRequest<IReadOnlyList<DepartmentDto>>;
