using AddressBook.Application.Lookup.Departments.DTOs;
using MediatR;

namespace AddressBook.Application.Lookup.Departments.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(Guid Id) : IRequest<DepartmentDto>;
