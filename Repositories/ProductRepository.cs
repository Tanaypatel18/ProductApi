using Microsoft.EntityFrameworkCore;
using myFirstWebApi.Data;
using myFirstWebApi.DTOs;
using myFirstWebApi.Models;

namespace myFirstWebApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Product> products, int totalCount)> GetAllAsync(ProductQueryDto query)
    {
        // Start with all products
        var dbQuery = _context.Products.AsQueryable();

        // Filter by search keyword
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            dbQuery = dbQuery.Where(p =>
                p.Name.Contains(query.Search) ||
                p.Description.Contains(query.Search));
        }

        // Filter by price range
        if (query.MinPrice.HasValue)
            dbQuery = dbQuery.Where(p => p.Price >= query.MinPrice.Value);

        if (query.MaxPrice.HasValue)
            dbQuery = dbQuery.Where(p => p.Price <= query.MaxPrice.Value);

        // Get total count BEFORE pagination
        var totalCount = await dbQuery.CountAsync();

        // Sorting
        dbQuery = query.SortBy?.ToLower() switch
        {
            "price" => query.SortOrder == "desc"
                ? dbQuery.OrderByDescending(p => p.Price)
                : dbQuery.OrderBy(p => p.Price),

            "stock" => query.SortOrder == "desc"
                ? dbQuery.OrderByDescending(p => p.Stock)
                : dbQuery.OrderBy(p => p.Stock),

            "name" => query.SortOrder == "desc"
                ? dbQuery.OrderByDescending(p => p.Name)
                : dbQuery.OrderBy(p => p.Name),

            _ => dbQuery.OrderBy(p => p.Id) // default sort
        };

        // Pagination — skip and take
        var products = await dbQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return false;

        _context.Products.Remove(product);
        return true;
    }

    public async Task SaveAsync() =>
        await _context.SaveChangesAsync();
}