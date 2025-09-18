using Microsoft.IdentityModel.Tokens;
namespace TicketHive.Application.Common.Interfaces;

public interface IRsaKeyStore
{
    string KeyId { get; } 
    RsaSecurityKey GetPrivateKey();
    RsaSecurityKey GetPublicKey();
}