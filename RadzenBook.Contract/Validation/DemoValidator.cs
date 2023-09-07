using FluentValidation;
using RadzenBook.Contract.DTO;

namespace RadzenBook.Contract.Validation;

public class DemoValidator : AbstractValidator<DemoInputDto>
{
    public DemoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Name).MaximumLength(50).WithMessage("Name must not exceed 50 characters");
        RuleFor(x => x.Name).MinimumLength(3).WithMessage("Name must be at least 3 characters");
        RuleFor(x => x.Description).MaximumLength(200).WithMessage("Description must not exceed 200 characters");
    }
}