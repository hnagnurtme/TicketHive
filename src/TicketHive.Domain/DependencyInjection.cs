using Microsoft.Extensions.DependencyInjection;

namespace TicketHive.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        // Thường Domain không có service phụ thuộc ngoài
        // nhưng tạo để giữ consistency
        return services;
    }
}
