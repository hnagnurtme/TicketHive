using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using FluentValidation;

namespace TicketHive.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Đăng ký MediatR: scan toàn bộ assembly Application
        services.AddMediatR(Assembly.GetExecutingAssembly());

        // Nếu bạn dùng FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    

        return services;
    }
}
