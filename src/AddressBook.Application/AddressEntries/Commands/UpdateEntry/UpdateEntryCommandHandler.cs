using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using MediatR;

namespace AddressBook.Application.AddressEntries.Commands.UpdateEntry;

public class UpdateEntryCommandHandler : IRequestHandler<UpdateEntryCommand>
{
    private readonly IAddressEntryRepository _repository;
    private readonly IApplicationDbContext _context;

    public UpdateEntryCommandHandler(
        IAddressEntryRepository repository,
        IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(UpdateEntryCommand request, CancellationToken cancellationToken)
    {
        var entry = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(AddressEntry), request.Id);

        if (await _repository.EmailExistsAsync(request.Email, excludeId: request.Id, cancellationToken: cancellationToken))
            throw new ConflictException($"An entry with email '{request.Email}' already exists.");

        entry.Update(
            request.FullName,
            request.JobId,
            request.DepartmentId,
            request.MobileNumber,
            request.DateOfBirth,
            request.Address,
            request.Email,
            request.PhotoUrl);

        _repository.Update(entry);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
