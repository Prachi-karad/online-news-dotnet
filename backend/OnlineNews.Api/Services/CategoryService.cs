using Microsoft.EntityFrameworkCore;
using OnlineNews.Api.Data;
using OnlineNews.Api.Middleware;
using OnlineNews.Api.Models;

namespace OnlineNews.Api.Services;

public class CategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db) => _db = db;

    public Task<List<Category>> GetAllAsync() =>
        _db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();

    public async Task<Category> GetAsync(int id) =>
        await _db.Categories.FindAsync(id) ?? throw new NotFoundException($"Category not found with id {id}");

    public async Task<Category> CreateAsync(Category category)
    {
        var name = category.Name.Trim();
        if (await _db.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower()))
            throw new BadRequestException($"Category '{name}' already exists");

        category.Id = 0;
        category.Name = name;
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return category;
    }

    public async Task DeleteAsync(int id)
    {
        var category = await GetAsync(id);
        if (await _db.Articles.AnyAsync(a => a.CategoryId == id))
            throw new BadRequestException("Cannot delete a category that still has articles");
        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
    }
}
