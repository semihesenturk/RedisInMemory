using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InmemoryApp.Web.Controllers;

public class ProductController(IMemoryCache memoryCache) : Controller
{
    // GET
    public IActionResult Index()
    {
        if (!memoryCache.TryGetValue("time", out string timeCache))
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.High
            };
            
            memoryCache.Set("time", DateTime.Now.ToString(), options);
        }
        
        return View();
    }

    public IActionResult Show()
    {
        memoryCache.TryGetValue("time", out string timeCache);
        
        ViewBag.Zaman = timeCache;
        return View();
    }
}