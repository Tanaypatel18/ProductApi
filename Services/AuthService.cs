using Microsoft.EntityFrameworkCore;
using myFirstWebApi.Data;
using myFirstWebApi.DTOs;
using myFirstWebApi.Models;

namespace myFirstWebApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<string?> Register(RegisterDto dto)
    {
        // Check if email already exists
        var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists) return null;

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return _jwtService.GenerateToken(user);
    }

    public async Task<string?> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return null;

        var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!passwordValid) return null;

        return _jwtService.GenerateToken(user);
    }

    public async Task<string?> RegisterWithRole(RegisterDto dto, string role)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists) return null;

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return _jwtService.GenerateToken(user);
    }

}