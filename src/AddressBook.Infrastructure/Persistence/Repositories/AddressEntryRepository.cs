using AddressBook.Application.AddressEntries.DTOs;
using AddressBook.Application.Common.Interfaces;
using AddressBook.Application.Common.Models;
using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Persistence.Repositories;

public class AddressEntryRepository : IAddressEntryRepository
{
    private readonly ApplicationDbContext _context;

    public AddressEntryRepository(ApplicationDbContext context) => _context = context;

    public async Task<AddressEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.AddressEntries
            .Include(e => e.Job)
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<PaginatedResult<AddressEntryListItemDto>> GetPagedAsync(
        AddressEntryFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AddressEntries.AsNoTracking();

        // Global search
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.Trim().ToLower();
            query = query.Where(e =>
                e.FullName.ToLower().Contains(term) ||
                e.Email.ToLower().Contains(term) ||
                e.MobileNumber.Contains(term));
        }

        // Field-specific filters
        if (!string.IsNullOrWhiteSpace(filter.FullName))
            query = query.Where(e => e.FullName.ToLower().Contains(filter.FullName.Trim().ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(e => e.Email.ToLower().Contains(filter.Email.Trim().ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.MobileNumber))
            query = query.Where(e => e.MobileNumber.Contains(filter.MobileNumber.Trim()));

        if (!string.IsNullOrWhiteSpace(filter.Address))
            query = query.Where(e => e.Address.ToLower().Contains(filter.Address.Trim().ToLower()));

        if (filter.JobId.HasValue)
            query = query.Where(e => e.JobId == filter.JobId.Value);

        if (filter.DepartmentId.HasValue)
            query = query.Where(e => e.DepartmentId == filter.DepartmentId.Value);

        if (filter.DobFrom.HasValue)
            query = query.Where(e => e.DateOfBirth >= filter.DobFrom.Value);

        if (filter.DobTo.HasValue)
            query = query.Where(e => e.DateOfBirth <= filter.DobTo.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, filter.SortBy, filter.SortDescending);

        // Project to anonymous type first (EF translates to optimized SQL with JOIN for Job/Department)
        // Age is computed in memory since it's a derived value that SQL can't express cleanly
        var raw = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(e => new
            {
                e.Id,
                e.FullName,
                JobName = e.Job.Name,
                DepartmentName = e.Department.Name,
                e.MobileNumber,
                e.Email,
                e.DateOfBirth,
                e.PhotoUrl
            })
            .ToListAsync(cancellationToken);

        var items = raw.Select(e => new AddressEntryListItemDto
        {
            Id = e.Id,
            FullName = e.FullName,
            JobName = e.JobName,
            DepartmentName = e.DepartmentName,
            MobileNumber = e.MobileNumber,
            Email = e.Email,
            Age = ComputeAge(e.DateOfBirth),
            PhotoUrl = e.PhotoUrl
        }).ToList();

        return new PaginatedResult<AddressEntryListItemDto>(items, totalCount, filter.PageNumber, filter.PageSize);
    }

    public async Task<IReadOnlyList<AddressEntryListItemDto>> GetAllForExportAsync(CancellationToken cancellationToken = default)
    {
        var raw = await _context.AddressEntries
            .AsNoTracking()
            .OrderBy(e => e.FullName)
            .Select(e => new
            {
                e.Id,
                e.FullName,
                JobName = e.Job.Name,
                DepartmentName = e.Department.Name,
                e.MobileNumber,
                e.Email,
                e.DateOfBirth,
                e.PhotoUrl
            })
            .ToListAsync(cancellationToken);

        return raw.Select(e => new AddressEntryListItemDto
        {
            Id = e.Id,
            FullName = e.FullName,
            JobName = e.JobName,
            DepartmentName = e.DepartmentName,
            MobileNumber = e.MobileNumber,
            Email = e.Email,
            Age = ComputeAge(e.DateOfBirth),
            PhotoUrl = e.PhotoUrl
        }).ToList();
    }

    public async Task AddAsync(AddressEntry entity, CancellationToken cancellationToken = default)
        => await _context.AddressEntries.AddAsync(entity, cancellationToken);

    public void Update(AddressEntry entity)
        => _context.AddressEntries.Update(entity);

    public void Remove(AddressEntry entity)
        => _context.AddressEntries.Remove(entity);

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default)
        => await _context.AddressEntries
            .AsNoTracking()
            .AnyAsync(e => e.Email == email && (excludeId == null || e.Id != excludeId), cancellationToken);

    private static IQueryable<AddressEntry> ApplySorting(
        IQueryable<AddressEntry> query, string? sortBy, bool descending) =>
        sortBy?.ToLower() switch
        {
            "email"       => descending ? query.OrderByDescending(e => e.Email)          : query.OrderBy(e => e.Email),
            "dateofbirth" => descending ? query.OrderByDescending(e => e.DateOfBirth)    : query.OrderBy(e => e.DateOfBirth),
            "mobilenumber"=> descending ? query.OrderByDescending(e => e.MobileNumber)   : query.OrderBy(e => e.MobileNumber),
            "address"     => descending ? query.OrderByDescending(e => e.Address)        : query.OrderBy(e => e.Address),
            "job"         => descending ? query.OrderByDescending(e => e.Job.Name)       : query.OrderBy(e => e.Job.Name),
            "department"  => descending ? query.OrderByDescending(e => e.Department.Name): query.OrderBy(e => e.Department.Name),
            "createdat"   => descending ? query.OrderByDescending(e => e.CreatedAt)      : query.OrderBy(e => e.CreatedAt),
            _             => descending ? query.OrderByDescending(e => e.FullName)       : query.OrderBy(e => e.FullName)
        };

    private static int ComputeAge(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow.Date;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}
