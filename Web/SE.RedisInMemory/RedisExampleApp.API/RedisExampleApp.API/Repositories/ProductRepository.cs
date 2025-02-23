using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Models;

namespace RedisExampleApp.API.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<List<Product>> GetAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        
        return product;
    }
}