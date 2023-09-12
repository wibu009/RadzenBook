using FluentValidation;
using RadzenBook.Common.Enums;
using RadzenBook.Contract.DTO.Demo;

namespace RadzenBook.Contract.Validations.Demo;

public class DemoUpdateValidator : AbstractValidator<DemoUpdateDto>
{
    public DemoUpdateValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters");
        RuleFor(x => x.DemoEnum)
            .Must(x => Enum.TryParse<DemoEnum>(x, out _)).WithMessage("DemoEnum is not valid");
    }
}