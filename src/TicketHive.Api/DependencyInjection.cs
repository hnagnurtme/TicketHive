using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace TicketHive.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // Controllers
        services.AddControllers(options =>
        {
            options.Filters.Add<TicketHive.Api.Filters.ValidationFilter>();
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .SelectMany(x => x.Value?.Errors ?? Enumerable.Empty<Microsoft.AspNetCore.Mvc.ModelBinding.ModelError>())
                    .Select(x => new
                    {
                        ErrorCode = "VALIDATION_ERROR",
                        Message = x.ErrorMessage
                    })
                    .ToList();

                var response = new TicketHive.Api.Common.ApiResponse<object>
                {
                    Success = false,
                    Message = errors.FirstOrDefault()?.Message ?? "Validation error",
                    StatusCode = 400,
                    Data = null,
                    Meta = null,
                    ErrorCode = errors.FirstOrDefault()?.ErrorCode ?? "VALIDATION_ERROR"
                };
                return new ObjectResult(response) { StatusCode = 400 };
            };
        });

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TicketHive API",
                Version = "v1",
                Description = "TicketHive is a robust platform for event ticket management, providing secure authentication, user management, and seamless event operations. This API enables integration with TicketHive's core features, supporting both internal and third-party applications.",
                TermsOfService = new Uri("https://tickethive.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "TicketHive Support",
                    Email = "support@tickethive.com",
                    Url = new Uri("https://tickethive.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Nhập 'Bearer {token}'"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            c.EnableAnnotations();
        });


        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        });

        return services;
    }
    // configure AutoMapper
    public static IServiceCollection AddMappings(this IServiceCollection services)
{
    var loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();

    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddMaps(typeof(Program).Assembly); // Sử dụng assembly cụ thể
    }, loggerFactory);

    services.AddSingleton(config.CreateMapper());
    return services;
}
}
