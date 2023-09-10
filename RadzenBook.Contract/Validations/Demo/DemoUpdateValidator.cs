using FluentValidation;
using RadzenBook.Contract.DTO.Demo;

namespace RadzenBook.Contract.Validations.Demo;

public class DemoUpdateValidator : AbstractValidator<DemoUpdateDto>
{
    public DemoUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage("Name must be at least 3 characters")
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters");
    }
}