using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class SetTypeController(RedisService redisService) : Controller
{
    private readonly IDatabase _db = redisService.GetDatabase(2);
    private string _listKey = "setNames";

    // GET
    public async Task<IActionResult> Index()
    {
        HashSet<string> hashNames = [];

        if (_db.KeyExists(_listKey))
        {
            (await _db.SetMembersAsync(_listKey)).ToList().ForEach(s=>
                hashNames.Add(s.ToString())); ;
        }
        
        return View(hashNames);
    }

    [HttpPost]
    public async Task<IActionResult> Add(string name)
    {
        _db.KeyExpire(_listKey, TimeSpan.FromMinutes(5));
        await _db.SetAddAsync(_listKey, name);
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteItem(string name)
    {
        await _db.SetRemoveAsync(_listKey, name);
        return RedirectToAction(nameof(Index));
    }
}