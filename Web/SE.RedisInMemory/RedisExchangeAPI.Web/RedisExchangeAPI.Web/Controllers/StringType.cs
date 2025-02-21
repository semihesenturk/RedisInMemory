using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class StringType(RedisService redis) : Controller
{
    private readonly IDatabase db = redis.GetDatabase(0);

    // GET
    public async Task<IActionResult> Index()
    {
        await db.StringSetAsync("name", "Semih Esentürk");
        await db.StringSetAsync("ziyaretçi", 100);

        return View();
    }

    public async Task<IActionResult> Show()
    {
        var value = await db.StringGetAsync("name");
        await db.StringIncrementAsync("ziyaretçi", 1);
        // await db.StringDecrementAsync("ziyaretci", 1);

        if (value.HasValue)
            ViewBag.Name = value;

        return View();
    }
}