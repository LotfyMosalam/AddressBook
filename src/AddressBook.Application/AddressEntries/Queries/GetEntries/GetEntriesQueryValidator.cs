using FluentValidation;

namespace AddressBook.Application.AddressEntries.Queries.GetEntries;

public class GetEntriesQueryValidator : AbstractValidator<GetEntriesQuery>
{
    private static readonly HashSet<string> ValidSortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "FullName", "Email", "MobileNumber", "DateOfBirth", "Address", "Job", "Department", "CreatedAt"
    };

    public GetEntriesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.SortBy)
            .Must(s => ValidSortFields.Contains(s))
            .WithMessage($"SortBy must be one of: {string.Join(", ", ValidSortFields)}.")
            .When(x => !string.IsNullOrEmpty(x.SortBy));
    }
}
