using FluentValidation;

namespace AddressBook.Application.Lookup.Jobs.Commands.CreateJob;

public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Job name is required.")
            .MaximumLength(100).WithMessage("Job name must not exceed 100 characters.");
    }
}
