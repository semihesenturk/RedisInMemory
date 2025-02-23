using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class SortedSetTypeController(RedisService redisService) : Controller
{
    private readonly IDatabase _db = redisService.GetDatabase(3);
    private string listKey = "sortedsetnames";

    // GET
    public IActionResult Index()
    {
        var list = new HashSet<string>();
        if (_db.KeyExists(listKey))
        {
            _db.SortedSetRangeByRank(listKey, order:Order.Descending).ToList().ForEach(x 
                => list.Add(x.ToString()));
            
            //Show with score info
            // _db.SortedSetScan(listKey).ToList().ForEach(s =>
            //     list.Add(s.ToString()));
        }

        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> Add(string value, int score)
    {
        await _db.SortedSetAddAsync(listKey, value, score);
        _db.KeyExpire(listKey, TimeSpan.FromMinutes(1));

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(string value)
    {
        if (_db.KeyExists(listKey))
        {
            await _db.SortedSetRemoveAsync(listKey, value);
        }

        return RedirectToAction("Index");
    }
}