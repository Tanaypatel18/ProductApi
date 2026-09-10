using myFirstWebApi.DTOs;
using myFirstWebApi.Models;
using myFirstWebApi.Repositories;

namespace myFirstWebApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PagedResultDto<Product>> GetAllAsync(ProductQueryDto query)
    {
        _logger.LogInformation("Getting products - Page: {Page}, PageSize: {PageSize}, Search: {Search}",
            query.Page, query.PageSize, query.Search);

        var (products, totalCount) = await _repository.GetAllAsync(query);

        var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new PagedResultDto<Product>
        {
            Data = products,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasNextPage = query.Page < totalPages,
            HasPreviousPage = query.Page > 1
        };
    }

    public async Task<Product?> GetByIdAsync(int id) =>
        await _repository.GetByIdAsync(id);

    public async Task<Product> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

        await _repository.CreateAsync(product);
        await _repository.SaveAsync();

        _logger.LogInformation("Product created: {Name}", product.Name);

        return product;
    }

    public async Task<Product?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return null;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;

        await _repository.UpdateAsync(product);
        await _repository.SaveAsync();

        _logger.LogInformation("Product updated: {Id}", id);

        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result) return false;

        await _repository.SaveAsync();

        _logger.LogInformation("Product deleted: {Id}", id);

        return true;
    }
}