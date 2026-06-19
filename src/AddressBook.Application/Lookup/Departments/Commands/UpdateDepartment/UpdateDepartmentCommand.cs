using MediatR;

namespace AddressBook.Application.Lookup.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(Guid Id, string Name) : IRequest;
