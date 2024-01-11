namespace RadzenBook.Application.Identity.Auth;

public class RegisterRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
}

public class RegisterRequestValidator : CustomValidator<RegisterRequest>
{
    public RegisterRequestValidator(IStringLocalizer<RegisterRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(t["Username is required"])
            .MinimumLength(3).WithMessage(t["Username must be at least {0} characters long", 3])
            .MaximumLength(50).WithMessage(t["Username must not exceed {0} characters", 50]);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(t["Password is required"])
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$").WithMessage(
                t["Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number and one special character"]);
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(t["Confirm Password is required"])
            .Equal(x => x.Password).WithMessage(t["Confirm Password must match Password"]);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(t["Email is required"])
            .EmailAddress().WithMessage(t["Email is not valid"])
            .MaximumLength(50).WithMessage(t["Email must not exceed {0} characters", 50]);
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage(t["Display Name is required"])
            .MinimumLength(3).WithMessage(t["Display Name must be at least {0} characters long", 3])
            .MaximumLength(50).WithMessage(t["Display Name must not exceed {0} characters", 50]);
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(0[1-9]{1}[0-9]{8}|0[1-9]{1}[0-9]{9})$").WithMessage(t["Phone Number is not valid"]);
    }
}