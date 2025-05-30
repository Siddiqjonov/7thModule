using ContactManager.Application.Dtos;
using FluentValidation;

namespace ContactManager.Application.FluentValidation;

public class UserLogInDtoValidator : AbstractValidator<UserLogInDto>
{
    public UserLogInDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
