using FluentValidation;

namespace AsaasBlazorAuthentication.Application.Subscribers.UpdateSubscriber;

public sealed class UpdateSubscriberInputModelValidator : AbstractValidator<UpdateSubscriberInputModel>
{
    public UpdateSubscriberInputModelValidator()
    {
        RuleFor(r => r.Name)
            .MinimumLength(3).WithMessage("Name must have a minimum of 3 characters")
            .MaximumLength(50).WithMessage("Name must have a maximum of 50 characters");

        RuleFor(r => r.PhoneNumber)
            .MinimumLength(10).WithMessage("PhoneNumber must be valid")
            .MaximumLength(16).WithMessage("PhoneNumber must have a maximum of 16 characters");
    }
}