using myFirstWebApi.Models;

namespace myFirstWebApi.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Only seed if no admin exists
        if (!context.Users.Any(u => u.Role == "Admin"))
        {
            var admin = new User
            {
                Name = "Super Admin",
                Email = "admin@myapi.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin"
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}