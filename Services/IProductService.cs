using myFirstWebApi.DTOs;
using myFirstWebApi.Models;

namespace myFirstWebApi.Services;

public interface IProductService
{
    List<Product> GetAll();
    Product? GetById(int id);
    Product Create(CreateProductDto dto);
    Product? Update(int id, UpdateProductDto dto);
    bool Delete(int id);
}