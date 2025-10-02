using FluentValidation;

namespace TicketHive.Application.Tickets;

public class GetTicketsQueryValidator : AbstractValidator<GetTicketsQuery>
{
    public GetTicketsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(x => x.SortBy)
            .Must(BeValidSortField)
            .When(x => !string.IsNullOrEmpty(x.SortBy))
            .WithMessage("Invalid sort field. Allowed values: Name, Type, Price, CreatedAt, SortOrder, IsActive");

        RuleFor(x => x.SortDirection)
            .Must(BeValidSortDirection)
            .When(x => !string.IsNullOrEmpty(x.SortDirection))
            .WithMessage("Invalid sort direction. Allowed values: asc, desc");
    }

    private static bool BeValidSortField(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy)) return true;
        
        var allowedFields = new[] { "Name", "Type", "Price", "CreatedAt", "SortOrder", "IsActive" };
        return allowedFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
    }

    private static bool BeValidSortDirection(string? sortDirection)
    {
        if (string.IsNullOrEmpty(sortDirection)) return true;
        
        var allowedDirections = new[] { "asc", "desc" };
        return allowedDirections.Contains(sortDirection, StringComparer.OrdinalIgnoreCase);
    }
}