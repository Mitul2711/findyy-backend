using findyy.DTO.Auth;
using findyy.Services.Auth.Interface;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto dto)
    {
        var token = await _authService.RegisterAsync(dto);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null) return Unauthorized();
        return Ok(result);
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        var response = await _authService.VerifyEmailAsync(token);
        if (!response.Status)
            return BadRequest(response);

        return Ok(response);
    }

}
