using myFirstWebApi.DTOs;
using myFirstWebApi.Models;
using myFirstWebApi.Repositories;

namespace myFirstWebApi.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();

        return categories.Select(c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            ProductCount = c.Products.Count
        }).ToList();
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return null;

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ProductCount = category.Products.Count
        };
    }

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _repository.CreateAsync(category);
        await _repository.SaveAsync();

        _logger.LogInformation("Category created: {Name}", category.Name);

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ProductCount = 0
        };
    }

    public async Task<CategoryResponseDto?> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return null;

        category.Name = dto.Name;
        category.Description = dto.Description;

        await _repository.UpdateAsync(category);
        await _repository.SaveAsync();

        _logger.LogInformation("Category updated: {Id}", id);

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ProductCount = category.Products.Count
        };
    }

    public async Task<(bool success, string message)> DeleteAsync(int id)
    {
        var exists = await _repository.ExistsAsync(id);
        if (!exists)
            return (false, "Category not found");

        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
            return (false, "Cannot delete category that has products");

        await _repository.SaveAsync();

        _logger.LogInformation("Category deleted: {Id}", id);

        return (true, "Category deleted successfully");
    }
}