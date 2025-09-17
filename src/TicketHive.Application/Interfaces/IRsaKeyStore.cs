using Microsoft.IdentityModel.Tokens;
namespace TicketHive.Application.Interfaces;

public interface IRsaKeyStore
{
    string KeyId { get; } 
    RsaSecurityKey GetPrivateKey();
    RsaSecurityKey GetPublicKey();
}