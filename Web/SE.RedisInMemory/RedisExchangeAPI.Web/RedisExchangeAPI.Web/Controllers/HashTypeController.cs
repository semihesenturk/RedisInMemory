using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class HashTypeController(RedisService redisService) : Controller
{
    private readonly IDatabase _db = redisService.GetDatabase(4);
    private readonly string hashKey = "hashNames";

    // GET
    public async Task<IActionResult> Index()
    {
        var list = new Dictionary<string, string>();
        
        if (_db.KeyExists(hashKey))
        {
            (await _db.HashGetAllAsync(hashKey)).ToList().ForEach(x 
                => list.Add(x.Name.ToString(), x.Value.ToString()));
        }
        
        return View(list);
    }


    [HttpPost]
    public async Task<IActionResult> Add(string name, string value)
    {
        await _db.HashSetAsync(hashKey, name, value);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(string key)
    {
        await _db.HashDeleteAsync(hashKey, key);
        return RedirectToAction("Index");
    }
    
}