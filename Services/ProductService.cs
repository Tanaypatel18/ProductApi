using myFirstWebApi.DTOs;
using myFirstWebApi.Models;
using myFirstWebApi.Repositories;

namespace myFirstWebApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public List<Product> GetAll() =>
        _repository.GetAll();

    public Product? GetById(int id) =>
        _repository.GetById(id);

    public Product Create(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

        _repository.Create(product);
        _repository.Save();

        return product;
    }

    public Product? Update(int id, UpdateProductDto dto)
    {
        var product = _repository.GetById(id);
        if (product == null) return null;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;

        _repository.Update(product);
        _repository.Save();

        return product;
    }

    public bool Delete(int id)
    {
        var result = _repository.Delete(id);
        if (!result) return false;

        _repository.Save();
        return true;
    }
}