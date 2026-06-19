using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using MediatR;

namespace AddressBook.Application.AddressEntries.Commands.DeleteEntry;

public class DeleteEntryCommandHandler : IRequestHandler<DeleteEntryCommand>
{
    private readonly IAddressEntryRepository _repository;
    private readonly IApplicationDbContext _context;

    public DeleteEntryCommandHandler(
        IAddressEntryRepository repository,
        IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(DeleteEntryCommand request, CancellationToken cancellationToken)
    {
        var entry = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(AddressEntry), request.Id);

        _repository.Remove(entry);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
