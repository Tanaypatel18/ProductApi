using myFirstWebApi.DTOs;
using myFirstWebApi.Models;

namespace myFirstWebApi.Repositories;

public interface IProductRepository
{
    Task<(List<Product> products, int totalCount)> GetAllAsync(ProductQueryDto query);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
    Task SaveAsync();
}