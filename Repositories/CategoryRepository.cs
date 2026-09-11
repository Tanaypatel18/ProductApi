using Microsoft.EntityFrameworkCore;
using myFirstWebApi.Data;
using myFirstWebApi.Models;

namespace myFirstWebApi.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync() =>
        await _context.Categories
            .Include(c => c.Products) // load products with category
            .ToListAsync();

    public async Task<Category?> GetByIdAsync(int id) =>
        await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Categories.AnyAsync(c => c.Id == id);

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        return category;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null) return false;

        // cannot delete category that has products
        if (category.Products.Any()) return false;

        _context.Categories.Remove(category);
        return true;
    }

    public async Task SaveAsync() =>
        await _context.SaveChangesAsync();
}   