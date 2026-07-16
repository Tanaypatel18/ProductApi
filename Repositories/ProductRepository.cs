using myFirstWebApi.Data;
using myFirstWebApi.Models;

namespace myFirstWebApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Product> GetAll() =>
        _context.Products.ToList();

    public Product? GetById(int id) =>
        _context.Products.FirstOrDefault(p => p.Id == id);

    public Product Create(Product product)
    {
        _context.Products.Add(product);
        return product;
    }

    public Product Update(Product product)
    {
        _context.Products.Update(product);
        return product;
    }

    public bool Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return false;

        _context.Products.Remove(product);
        return true;
    }

    public void Save() => _context.SaveChanges();
}