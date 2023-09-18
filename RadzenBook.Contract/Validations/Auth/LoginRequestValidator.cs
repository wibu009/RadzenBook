using FluentValidation;
using Microsoft.Extensions.Localization;
using RadzenBook.Contract.DTO.Auth;

namespace RadzenBook.Contract.Validations.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator(IStringLocalizer<LoginRequestValidator> t)
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(t["Username is required"]);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(t["Password is required"]);
    }
}