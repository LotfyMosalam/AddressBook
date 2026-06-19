using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using MediatR;

namespace AddressBook.Application.Lookup.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly IDepartmentRepository _repository;
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IDepartmentRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.Id);

        if (await _repository.NameExistsAsync(request.Name, excludeId: request.Id, cancellationToken: cancellationToken))
            throw new ConflictException($"A department named '{request.Name}' already exists.");

        department.UpdateName(request.Name);
        _repository.Update(department);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
