using FluentValidation;

namespace AsaasBlazorAuthentication.Application.Users.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(r => r.Name)
            .MinimumLength(3).WithMessage("Name must have a minimum of 3 characters")
            .MaximumLength(50).WithMessage("Name must have a maximum of 50 characters");

        RuleFor(r => r.Email)
            .EmailAddress().WithMessage("Email must be valid")
            .MinimumLength(5).WithMessage("Email must have a minimum of 5 characters")
            .MaximumLength(100).WithMessage("Email must have a maximum of 100 characters");

        RuleFor(r => r.PhoneNumber)
            .MinimumLength(10).WithMessage("PhoneNumber must be valid")
            .MaximumLength(16).WithMessage("PhoneNumber must have a maximum of 16 characters");

        RuleFor(r => r.Password)
            .MinimumLength(8).WithMessage("Password must have a minimum of 8 characters")
            .MaximumLength(100).WithMessage("Password must have a maximum of 100 characters");

        RuleFor(r => r.PasswordCheck)
            .MinimumLength(8).WithMessage("Password must have a minimum of 8 characters")
            .MaximumLength(100).WithMessage("Password must have a maximum of 100 characters")
            .Must((r, pass) => r.Password == pass).WithMessage("Passwords must be equal");
    }
}