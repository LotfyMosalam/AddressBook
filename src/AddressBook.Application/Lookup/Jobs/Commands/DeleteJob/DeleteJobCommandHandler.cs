using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Commands.DeleteJob;

public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand>
{
    private readonly IJobRepository _repository;
    private readonly IApplicationDbContext _context;

    public DeleteJobCommandHandler(IJobRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Job), request.Id);

        _repository.Remove(job);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
