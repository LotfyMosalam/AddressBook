using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Persistence.Repositories;

public class JobRepository : IJobRepository
{
    private readonly ApplicationDbContext _context;

    public JobRepository(ApplicationDbContext context) => _context = context;

    public async Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Jobs
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Job>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Jobs
            .AsNoTracking()
            .OrderBy(j => j.Name)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Job entity, CancellationToken cancellationToken = default)
        => await _context.Jobs.AddAsync(entity, cancellationToken);

    public void Update(Job entity)
        => _context.Jobs.Update(entity);

    public void Remove(Job entity)
        => _context.Jobs.Remove(entity);

    public async Task<bool> NameExistsAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default)
        => await _context.Jobs
            .AsNoTracking()
            .AnyAsync(j => j.Name == name && (excludeId == null || j.Id != excludeId), cancellationToken);
}
