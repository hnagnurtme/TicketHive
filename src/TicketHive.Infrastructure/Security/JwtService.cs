using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TicketHive.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TicketHive.Infrastructure.Security;

public class JwtService : IJwtService
{
    private readonly IRsaKeyStore _rsaKeyStore;
    private readonly IConfiguration _configuration;


    public JwtService(IRsaKeyStore rsaKeyStore, IConfiguration configuration)
    {
        _rsaKeyStore = rsaKeyStore;
        _configuration = configuration;
    }

    public string GenerateToken(
    IEnumerable<Claim> claims,
    TimeSpan? lifetime = null
)
    {
        var signingCredentials = new SigningCredentials(
            _rsaKeyStore.GetPrivateKey(),
            SecurityAlgorithms.RsaSha256
        );

        var issuer = _configuration["Jwt:Issuer"] ?? "TicketHive";
        var audience = _configuration["Jwt:Audience"] ?? "TicketHiveClients";

        var expires = DateTime.UtcNow.Add(lifetime ?? TimeSpan.FromHours(1));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials,
            notBefore: DateTime.UtcNow,
            issuer: issuer,
            audience: audience
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public ClaimsPrincipal? ValidateToken(string token)
    {
        var issuer = _configuration["Jwt:Issuer"] ?? "TicketHive";
        var audience = _configuration["Jwt:Audience"] ?? "TicketHiveClients";
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            IssuerSigningKey = _rsaKeyStore.GetPublicKey(),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }

}