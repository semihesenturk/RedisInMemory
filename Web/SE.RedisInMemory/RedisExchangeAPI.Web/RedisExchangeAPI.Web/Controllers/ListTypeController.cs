using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class ListTypeController(RedisService redisService) : Controller
{
    private readonly IDatabase _db = redisService.GetDatabase(1);
    private string _listKey = "names";

    // GET
    public async Task<IActionResult> Index()
    {
        var namesList = new List<string>();
        if (_db.KeyExists(_listKey))
        {
            (await _db.ListRangeAsync(_listKey)).ToList().ForEach(x =>
                namesList.Add(x.ToString()));
        }

        return View(namesList);
    }

    [HttpPost]
    public async Task<IActionResult> Add(string name)
    {
        //listenin sonuna eklemek
        await _db.ListRightPushAsync(_listKey, name);
        
        //listenin başına eklemek
        // await _db.ListLeftPushAsync(_listKey, name);
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> DeleteItem(string name)
    {
        //listenin en başındakini silmek için
        // await _db.ListLeftPopAsync(_listKey);

        //listenin en sonundakini silmek için
        // await _db.ListRightPopAsync(_listKey);
        
        await _db.ListRemoveAsync(_listKey, name);
        return RedirectToAction("Index");
    }
}