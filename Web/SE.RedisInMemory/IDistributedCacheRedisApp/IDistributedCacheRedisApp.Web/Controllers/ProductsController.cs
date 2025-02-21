using System.Net.Mime;
using System.Text;
using System.Text.Json;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers;

public class ProductsController(IDistributedCache distributedCache) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var cacheOptions = new DistributedCacheEntryOptions();
        cacheOptions.SetSlidingExpiration(TimeSpan.FromMinutes(10));
        cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        // await distributedCache.SetStringAsync("time", DateTime.Now.ToString(), cacheOptions);

        var product = new Product
        {
            Id = 2,
            Name = "Kalem2",
            Price = 120
        };

        var jsonProduct = JsonSerializer.Serialize(product);
        var byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

        await distributedCache.SetAsync("product:3", byteProduct, cacheOptions);

        // await distributedCache.SetStringAsync("product:2", jsonProduct, cacheOptions);
        return View();
    }

    public async Task<IActionResult> Show()
    {
        var getTime = await distributedCache.GetStringAsync("product:1");
        ViewBag.Time = getTime;
        return View();
    }

    public async Task<IActionResult> Remove()
    {
        await distributedCache.RemoveAsync("product:1");
        return View();
    }

    public async Task<IActionResult> ImageCache()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/car.webp");

        var imageBytes = await System.IO.File.ReadAllBytesAsync(path);
        await distributedCache.SetAsync("productImage:1", imageBytes);

        return View();
    }

    public async Task<IActionResult> GetImage()
    {
        var byteImage = await distributedCache.GetAsync("productImage:1");
        return File(byteImage, "image/webp");
    }
}