using FluentValidation;
using RadzenBook.Contract.DTO.Auth;

namespace RadzenBook.Contract.Validations.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MaximumLength(50).WithMessage("Password must not exceed 50 characters");
    }
}