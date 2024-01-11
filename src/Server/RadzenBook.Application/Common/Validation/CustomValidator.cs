using FluentValidation;

namespace RadzenBook.Application.Common.Validation;

public class CustomValidator<T> : AbstractValidator<T>
{
    public CustomValidator()
    {
        
    }
    
    public CustomValidator(IStringLocalizer<CustomValidator<T>> t)
    {
        RuleFor(x => x).NotNull().WithMessage(t["Request cannot be null"]);
    }
}