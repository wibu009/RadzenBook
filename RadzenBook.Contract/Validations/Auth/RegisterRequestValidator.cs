using FluentValidation;
using RadzenBook.Contract.DTO.Auth;

namespace RadzenBook.Contract.Validations.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$").WithMessage(
                "Password must be at least 8 characters, contain a lowercase letter, an uppercase letter, a number, and a special character");
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required")
            .Equal(x => x.Password).WithMessage("Password and Confirm Password must match");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid")
            .MaximumLength(50).WithMessage("Email must not exceed 50 characters");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required")
            .MaximumLength(50).WithMessage("First Name must not exceed 50 characters");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required")
            .MaximumLength(50).WithMessage("Last Name must not exceed 50 characters");
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone Number must not exceed 20 characters")
            .Matches(@"^(0[1-9]{1}[0-9]{8}|0[1-9]{1}[0-9]{9})$").WithMessage("Phone Number is not valid");
    }
}