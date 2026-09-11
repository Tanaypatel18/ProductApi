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

    public async Task<PagedResultDto<ProductResponseDto>> GetAllAsync(ProductQueryDto query)
    {
        _logger.LogInformation("Getting products - Page: {Page}, PageSize: {PageSize}, Search: {Search}",
            query.Page, query.PageSize, query.Search);

        var (products, totalCount) = await _repository.GetAllAsync(query);

        var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new PagedResultDto<ProductResponseDto>
        {
            Data = products.Select(MapToDto).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasNextPage = query.Page < totalPages,
            HasPreviousPage = query.Page > 1
        };
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return null;
        return MapToDto(product);
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId
        };

        await _repository.CreateAsync(product);
        await _repository.SaveAsync();

        // reload with category
        var created = await _repository.GetByIdAsync(product.Id);

        _logger.LogInformation("Product created: {Name}", product.Name);

        return MapToDto(created!);
    }

    public async Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return null;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;

        await _repository.UpdateAsync(product);
        await _repository.SaveAsync();

        // reload with category
        var updated = await _repository.GetByIdAsync(id);

        _logger.LogInformation("Product updated: {Id}", id);

        return MapToDto(updated!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result) return false;

        await _repository.SaveAsync();

        _logger.LogInformation("Product deleted: {Id}", id);

        return true;
    }

    private ProductResponseDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock,
        CreatedAt = product.CreatedAt,
        CategoryId = product.CategoryId,
        CategoryName = product.Category?.Name ?? string.Empty
    };
}