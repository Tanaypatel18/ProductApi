using Microsoft.AspNetCore.Mvc;
using myFirstWebApi.DTOs;
using myFirstWebApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace myFirstWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var token = await _authService.Register(dto);

        if (token == null)
            return BadRequest("Email already exists");

        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.Login(dto);

        if (token == null)
            return Unauthorized("Invalid email or password");

        return Ok(new { token });
    }

    [HttpPost("register-admin")]
    [Authorize(Roles = "Admin")] // ← only existing admin can create new admin
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto dto)
    {
        var token = await _authService.RegisterWithRole(dto, "Admin");
        if (token == null) return BadRequest("Email already exists");
        return Ok(new { token });
    }
}