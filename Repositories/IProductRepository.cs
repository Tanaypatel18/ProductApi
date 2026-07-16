using myFirstWebApi.Models;

namespace myFirstWebApi.Repositories;

public interface IProductRepository
{
    List<Product> GetAll();
    Product? GetById(int id);
    Product Create(Product product);
    Product Update(Product product);
    bool Delete(int id);
    void Save();
}