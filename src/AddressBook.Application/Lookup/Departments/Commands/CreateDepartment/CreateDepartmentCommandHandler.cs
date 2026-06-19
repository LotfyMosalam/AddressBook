using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using MediatR;

namespace AddressBook.Application.Lookup.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly IDepartmentRepository _repository;
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IDepartmentRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.NameExistsAsync(request.Name, cancellationToken: cancellationToken))
            throw new ConflictException($"A department named '{request.Name}' already exists.");

        var department = Department.Create(request.Name);
        await _repository.AddAsync(department, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return department.Id;
    }
}
