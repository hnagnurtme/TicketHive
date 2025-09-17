using TicketHive.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
namespace TicketHive.Infrastructure.Security;

public class RsaKeyStore : IRsaKeyStore
{
    public string KeyId { get; }
    private readonly RsaSecurityKey _privateKey;
    private readonly RsaSecurityKey _publicKey;

    public RsaKeyStore(string privatePemPath, string publicPemPath, string keyId)
    {
        KeyId = keyId;

        // Load priavte keys from PEM files
        var rsaPrivate = RSA.Create();
        rsaPrivate.ImportFromPem(File.ReadAllText(privatePemPath));
        _privateKey = new RsaSecurityKey(rsaPrivate) { KeyId = keyId };

        // Load public keys from PEM files
        var rsaPublic = RSA.Create();
        rsaPublic.ImportFromPem(File.ReadAllText(publicPemPath));
        _publicKey = new RsaSecurityKey(rsaPublic) { KeyId = keyId };
    }
    public RsaSecurityKey GetPrivateKey() => _privateKey;

    public RsaSecurityKey GetPublicKey() => _publicKey;
    
}