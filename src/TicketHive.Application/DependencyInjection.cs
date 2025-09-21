using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using MediatR;
using TicketHive.Application.Common;

namespace TicketHive.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Đăng ký MediatR: scan toàn bộ assembly Application
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Đăng ký tất cả FluentValidation validators trong assembly này
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Thêm ValidationBehavior vào pipeline của MediatR
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
