using Microsoft.AspNetCore.Mvc;
using OnlineNews.Api.Models;
using OnlineNews.Api.Services;

namespace OnlineNews.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService) => _categoryService = categoryService;

    [HttpGet]
    public Task<List<Category>> GetAll() => _categoryService.GetAllAsync();

    [HttpPost]
    public async Task<ActionResult<Category>> Create(Category category)
    {
        var created = await _categoryService.CreateAsync(category);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
}
