using MediatR;

namespace AddressBook.Application.Lookup.Departments.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(Guid Id) : IRequest;
