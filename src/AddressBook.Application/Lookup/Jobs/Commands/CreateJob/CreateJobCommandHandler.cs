using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using MediatR;

namespace AddressBook.Application.Lookup.Jobs.Commands.CreateJob;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Guid>
{
    private readonly IJobRepository _repository;
    private readonly IApplicationDbContext _context;

    public CreateJobCommandHandler(IJobRepository repository, IApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.NameExistsAsync(request.Name, cancellationToken: cancellationToken))
            throw new ConflictException($"A job named '{request.Name}' already exists.");

        var job = Job.Create(request.Name);
        await _repository.AddAsync(job, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
