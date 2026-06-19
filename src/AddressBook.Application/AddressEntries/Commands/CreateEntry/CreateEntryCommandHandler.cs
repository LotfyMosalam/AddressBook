using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using MediatR;

namespace AddressBook.Application.AddressEntries.Commands.CreateEntry;

public class CreateEntryCommandHandler : IRequestHandler<CreateEntryCommand, Guid>
{
    private readonly IAddressEntryRepository _repository;
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public CreateEntryCommandHandler(
        IAddressEntryRepository repository,
        IApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(CreateEntryCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.EmailExistsAsync(request.Email, cancellationToken: cancellationToken))
            throw new ConflictException($"An entry with email '{request.Email}' already exists.");

        var passwordHash = _passwordHasher.Hash(request.Password);

        var entry = AddressEntry.Create(
            request.FullName,
            request.JobId,
            request.DepartmentId,
            request.MobileNumber,
            request.DateOfBirth,
            request.Address,
            request.Email,
            passwordHash,
            request.PhotoUrl);

        await _repository.AddAsync(entry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entry.Id;
    }
}
