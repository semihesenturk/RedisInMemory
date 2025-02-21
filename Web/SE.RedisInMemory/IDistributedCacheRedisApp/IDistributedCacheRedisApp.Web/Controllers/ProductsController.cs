using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers;

public class ProductsController(IDistributedCache distributedCache) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var cacheOptions = new DistributedCacheEntryOptions();
        cacheOptions.SetSlidingExpiration(TimeSpan.FromSeconds(30));
        cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

        await distributedCache.SetStringAsync("time", DateTime.Now.ToString(), cacheOptions);
        return View();
    }

    public async Task<IActionResult> Show()
    {
        var getTime = await distributedCache.GetStringAsync("time");
        ViewBag.Time = getTime;
        return View();
    }

    public async Task<IActionResult> Remove()
    {
        await distributedCache.RemoveAsync("time");
        return View();
    }
}