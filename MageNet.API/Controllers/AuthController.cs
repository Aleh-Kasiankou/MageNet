using MageNetServices.Authentication.DTO;
using MageNetServices.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MageNet.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILoginHandler _loginHandler;

    public AuthController(ILoginHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginData loginData)
    {
        var authResult = await _loginHandler.TryLogInBackendUser(loginData);
        if (authResult.isAuthSuccessful)
        {
            return Ok(authResult.token);
        }

        return Unauthorized();
    }
}