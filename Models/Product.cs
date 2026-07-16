namespace myFirstWebApi.Models;

public class Product
{
    public int Id { get; set; }          // EF Core auto-increments this
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}