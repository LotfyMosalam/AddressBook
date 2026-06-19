using AddressBook.Application.Common.Interfaces;
using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Persistence.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context) => _context = context;

    public async Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Departments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Department entity, CancellationToken cancellationToken = default)
        => await _context.Departments.AddAsync(entity, cancellationToken);

    public void Update(Department entity)
        => _context.Departments.Update(entity);

    public void Remove(Department entity)
        => _context.Departments.Remove(entity);

    public async Task<bool> NameExistsAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default)
        => await _context.Departments
            .AsNoTracking()
            .AnyAsync(d => d.Name == name && (excludeId == null || d.Id != excludeId), cancellationToken);
}
