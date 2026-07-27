using myFirstWebApi.DTOs;

namespace myFirstWebApi.Services;

public interface IAuthService
{
    Task<string?> Register(RegisterDto dto);
    Task<string?> Login(LoginDto dto);
    Task<string?> RegisterWithRole(RegisterDto dto, string role);

}