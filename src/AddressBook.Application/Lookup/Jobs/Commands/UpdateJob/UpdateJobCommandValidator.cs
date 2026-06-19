using FluentValidation;

namespace AddressBook.Application.Lookup.Jobs.Commands.UpdateJob;

public class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
{
    public UpdateJobCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Job ID is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Job name is required.")
            .MaximumLength(100).WithMessage("Job name must not exceed 100 characters.");
    }
}
