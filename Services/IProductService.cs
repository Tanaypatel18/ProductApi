using myFirstWebApi.DTOs;
using myFirstWebApi.Models;

namespace myFirstWebApi.Services;

public interface IProductService
{
    Task<PagedResultDto<Product>> GetAllAsync(ProductQueryDto query);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(CreateProductDto dto);
    Task<Product?> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}