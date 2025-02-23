using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repositories;

namespace RedisExampleApp.API.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<List<Product>> GetAsync()
    {
        return await productRepository.GetAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await productRepository.GetByIdAsync(id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        return await productRepository.AddAsync(product);
    }
}