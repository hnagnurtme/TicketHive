using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketHive.Application.Interfaces.Repositories;
using TicketHive.Application.Interfaces;
using TicketHive.Infrastructure.Persistence;
using TicketHive.Infrastructure.Persistence.Repositories;
using TicketHive.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TicketHive.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddSecurity(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
            });
        });

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddSecurity(
        this IServiceCollection services, IConfiguration configuration)
    {
        var privatePemPath = configuration["Jwt:PrivateKeyPath"];
        var publicPemPath = configuration["Jwt:PublicKeyPath"];
        var keyId = configuration["Jwt:KeyId"];

        if (string.IsNullOrWhiteSpace(privatePemPath))
            throw new ArgumentNullException(nameof(privatePemPath), "Jwt:PrivateKeyPath is not configured.");
        if (string.IsNullOrWhiteSpace(publicPemPath))
            throw new ArgumentNullException(nameof(publicPemPath), "Jwt:PublicKeyPath is not configured.");
        if (string.IsNullOrWhiteSpace(keyId))
            throw new ArgumentNullException(nameof(keyId), "Jwt:KeyId is not configured.");

        // KeyStore giữ cặp key suốt vòng đời app
        var rsaKeyStore = new RsaKeyStore(privatePemPath, publicPemPath, keyId);
        services.AddSingleton<IRsaKeyStore>(rsaKeyStore);

        // JwtService và JwksProvider
        services.AddScoped<IJwksProvider, JwksProvider>();
        services.AddScoped<IJwtService, JwtService>();

        // Authentication với JWT Bearer
        var issuer = configuration["Jwt:Issuer"] ?? "TicketHive";
        var audience = configuration["Jwt:Audience"] ?? "TicketHiveClients";
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = rsaKeyStore.GetPublicKey()
                };
                options.RequireHttpsMetadata = false; // dev environment
            });
        Console.WriteLine("RSA Key Store initialized with Key ID: " + rsaKeyStore.KeyId);
        return services;
    }
}
