using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Commands.UpdateJob;

public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand>
{
    private readonly IJobRepository _repository;
    private readonly IApplicationDbContext _context;

    public UpdateJobCommandHandler(IJobRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Job), request.Id);

        if (await _repository.NameExistsAsync(request.Name, excludeId: request.Id, cancellationToken: cancellationToken))
            throw new ConflictException($"A job named '{request.Name}' already exists.");

        job.UpdateName(request.Name);
        _repository.Update(job);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
