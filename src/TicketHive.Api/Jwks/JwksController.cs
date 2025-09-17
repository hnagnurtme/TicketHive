
using Microsoft.AspNetCore.Mvc;
using TicketHive.Application.Interfaces;

namespace TicketHive.Api.Controllers.Jwks;

public class JwksController : ControllerBase
{
    private readonly IJwksProvider _jwksProvider;

    public JwksController(IJwksProvider jwksProvider)
    {
        _jwksProvider = jwksProvider;
    }

    [HttpGet("/.well-known/jwks.json")]
    public IActionResult GetJwks()
    {
        var jwks = _jwksProvider.GetJwks();
        return Ok(jwks);
    }
}

      

