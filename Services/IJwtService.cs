using myFirstWebApi.Models;

namespace myFirstWebApi.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}