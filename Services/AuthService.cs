using Microsoft.EntityFrameworkCore;
using myFirstWebApi.Data;
using myFirstWebApi.DTOs;
using myFirstWebApi.Models;

namespace myFirstWebApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IJwtService jwtService, ILogger<AuthService> logger)
    {
        _context = context;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<string?> Register(RegisterDto dto)
    {
        _logger.LogInformation("Register attempt for email: {Email}", dto.Email);

        var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists)
        {
            _logger.LogWarning("Register failed — email already exists: {Email}", dto.Email);
            return null;
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User registered successfully: {Email} with Role: {Role}", dto.Email, user.Role);

        return _jwtService.GenerateToken(user);
    }

    public async Task<string?> Login(LoginDto dto)
    {
        _logger.LogInformation("Login attempt for email: {Email}", dto.Email);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed — user not found: {Email}", dto.Email);
            return null;
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!passwordValid)
        {
            _logger.LogWarning("Login failed — wrong password for: {Email}", dto.Email);
            return null;
        }

        _logger.LogInformation("Login successful for: {Email}", dto.Email);

        return _jwtService.GenerateToken(user);
    }

    public async Task<string?> RegisterWithRole(RegisterDto dto, string role)
    {
        _logger.LogInformation("Register admin attempt for email: {Email}", dto.Email);

        var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists)
        {
            _logger.LogWarning("Register admin failed — email exists: {Email}", dto.Email);
            return null;
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Admin registered successfully: {Email}", dto.Email);

        return _jwtService.GenerateToken(user);
    }
}