using FluentValidation;

namespace TicketHive.Application.Authentication;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
        RuleFor(x => x.IpAddress)
            .NotEmpty()
            .WithMessage("IpAddress is required");
        RuleFor(x => x.UserAgent)
            .NotEmpty()
            .WithMessage("UserAgent is required");
    }
}