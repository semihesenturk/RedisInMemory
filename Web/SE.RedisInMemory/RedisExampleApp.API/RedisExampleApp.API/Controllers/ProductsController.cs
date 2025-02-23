using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Services;

namespace RedisExampleApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await productService.GetAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return Ok(await productService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] Product product)
    {
        return Ok(await productService.AddAsync(product));
    }
}