using System.Text.Json;
using RedisExampleApp.API.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;

namespace RedisExampleApp.API.Repositories;

public class ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService)
    : IProductRepository
{
    private readonly RedisService _redisService = redisService;
    private readonly IDatabase _cacheRepository = redisService.GetDatabase(2);
    private const string CacheKey = "Products";

    public async Task<List<Product>> GetAsync()
    {
        if (!await _cacheRepository.KeyExistsAsync(CacheKey))
        {
            return await LoadToCacheFromDbAsync();
        }

        var cachedProducts = await _cacheRepository.HashGetAllAsync(CacheKey);
        var products = cachedProducts.ToList().Select(item => JsonSerializer.Deserialize<Product>(item.Value)).ToList();

        return products;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        if (_cacheRepository.KeyExists(CacheKey))
        {
            var product = _cacheRepository.HashGetAsync(CacheKey, id).Result;
            return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
        }

        var products = await LoadToCacheFromDbAsync();
        return products.FirstOrDefault(s => s.Id == id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        var newProduct = await productRepository.AddAsync(product);

        if (_cacheRepository.KeyExists(CacheKey))
        {
            await _cacheRepository.HashSetAsync(CacheKey, newProduct.Id, JsonSerializer.Serialize(newProduct));
        }

        return newProduct;
    }

    private async Task<List<Product>> LoadToCacheFromDbAsync()
    {
        var products = await productRepository.GetAsync();
        products.ForEach(p => { _cacheRepository.HashSetAsync(CacheKey, p.Id, JsonSerializer.Serialize(p)); });

        return products;
    }
}