using FluentValidation;
namespace TicketHive.Application.Authentication;

public class RefreshTokenCommandValidator : AbstractValidator<GenerateRefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
        RuleFor(x => x.IpAddress)
            .NotEmpty()
            .WithMessage("IpAddress is required");
        RuleFor(x => x.UserAgent)
            .NotEmpty()
            .WithMessage("UserAgent is required");
    }
}