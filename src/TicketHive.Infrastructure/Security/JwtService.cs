using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TicketHive.Application.Interfaces;

namespace TicketHive.Infrastructure.Security;

public class JwtService : IJwtService
{
    private readonly IRsaKeyStore _rsaKeyStore;


    public JwtService(IRsaKeyStore rsaKeyStore)
    {
        _rsaKeyStore = rsaKeyStore;
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var signingCredentials = new SigningCredentials(_rsaKeyStore.GetPrivateKey(), SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: signingCredentials,
            notBefore: DateTime.UtcNow,
            issuer: "TicketHive",
            audience: "TicketHiveClients"
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "TicketHive",
            ValidateAudience = true,
            ValidAudience = "TicketHiveClients",
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