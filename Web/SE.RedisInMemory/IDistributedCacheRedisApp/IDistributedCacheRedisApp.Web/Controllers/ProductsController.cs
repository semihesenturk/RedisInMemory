using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers;

public class ProductsController(IDistributedCache distributedCache) : Controller
{
    private readonly IDistributedCache _distributedCache = distributedCache;

    // GET
    public IActionResult Index()
    {
        return View();
    }
}