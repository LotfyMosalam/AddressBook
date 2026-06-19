using AddressBook.Domain.Common;

namespace AddressBook.Domain.Entities;

public sealed class AddressEntry : BaseEntity
{
    private AddressEntry() { }

    public string FullName { get; private set; } = string.Empty;
    public Guid JobId { get; private set; }
    public Guid DepartmentId { get; private set; }
    public string MobileNumber { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public string Address { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string? PhotoUrl { get; private set; }

    // Computed — never stored; configured as NotMapped in EF configuration
    public int Age => ComputeAge(DateOfBirth);

    // Navigation properties
    public Job Job { get; private set; } = null!;
    public Department Department { get; private set; } = null!;

    public static AddressEntry Create(
        string fullName,
        Guid jobId,
        Guid departmentId,
        string mobileNumber,
        DateTime dateOfBirth,
        string address,
        string email,
        string password,
        string? photoUrl = null)
    {
        ValidateFields(fullName, jobId, departmentId, mobileNumber, dateOfBirth, address, email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

        return new AddressEntry
        {
            FullName = fullName.Trim(),
            JobId = jobId,
            DepartmentId = departmentId,
            MobileNumber = mobileNumber.Trim(),
            DateOfBirth = dateOfBirth.Date,
            Address = address.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Password = password,
            PhotoUrl = photoUrl?.Trim()
        };
    }

    public void Update(
        string fullName,
        Guid jobId,
        Guid departmentId,
        string mobileNumber,
        DateTime dateOfBirth,
        string address,
        string email,
        string? photoUrl)
    {
        ValidateFields(fullName, jobId, departmentId, mobileNumber, dateOfBirth, address, email);

        FullName = fullName.Trim();
        JobId = jobId;
        DepartmentId = departmentId;
        MobileNumber = mobileNumber.Trim();
        DateOfBirth = dateOfBirth.Date;
        Address = address.Trim();
        Email = email.Trim().ToLowerInvariant();
        PhotoUrl = photoUrl?.Trim();
    }

    public void ChangePassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));
        Password = password;
    }

    public void SetPhoto(string? photoUrl) => PhotoUrl = photoUrl?.Trim();

    // ─── private helpers ─────────────────────────────────────────────────────

    private static void ValidateFields(
        string fullName,
        Guid jobId,
        Guid departmentId,
        string mobileNumber,
        DateTime dateOfBirth,
        string address,
        string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName, nameof(fullName));
        ArgumentException.ThrowIfNullOrWhiteSpace(mobileNumber, nameof(mobileNumber));
        ArgumentException.ThrowIfNullOrWhiteSpace(address, nameof(address));
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        if (jobId == Guid.Empty)
            throw new ArgumentException("Job must be specified.", nameof(jobId));

        if (departmentId == Guid.Empty)
            throw new ArgumentException("Department must be specified.", nameof(departmentId));

        if (dateOfBirth.Date >= DateTime.UtcNow.Date)
            throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));
    }

    private static int ComputeAge(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow.Date;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;
        return age;
    }
}
