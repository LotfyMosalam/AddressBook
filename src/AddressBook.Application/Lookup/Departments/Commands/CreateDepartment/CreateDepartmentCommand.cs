using MediatR;

namespace AddressBook.Application.Lookup.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(string Name) : IRequest<Guid>;
