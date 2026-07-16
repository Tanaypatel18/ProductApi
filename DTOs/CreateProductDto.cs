using System.ComponentModel.DataAnnotations;

namespace myFirstWebApi.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "Name is required")]  //--attributes validation
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")] //--attributes
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")] //--attributes
    public string Description { get; set; } = string.Empty;

    [Required] //--attributes
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99")] //--attributes
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")] //--attributes
    public int Stock { get; set; }
}