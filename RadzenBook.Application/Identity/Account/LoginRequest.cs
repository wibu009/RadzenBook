using FluentValidation;
using Microsoft.Extensions.Localization;
using RadzenBook.Application.Common.Validation;

namespace RadzenBook.Application.Identity.Account;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginRequestValidator : CustomValidator<LoginRequest>
{
    public LoginRequestValidator(IStringLocalizer<LoginRequestValidator> t)
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(t["Username is required"]);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(t["Password is required"]);
    }
}