using Microsoft.EntityFrameworkCore;
using OnlineNews.Api.Data;
using OnlineNews.Api.Dtos;
using OnlineNews.Api.Middleware;
using OnlineNews.Api.Models;

namespace OnlineNews.Api.Services;

public class ArticleService
{
    private const int MaxPageSize = 50;

    private readonly AppDbContext _db;
    private readonly CategoryService _categoryService;

    public ArticleService(AppDbContext db, CategoryService categoryService)
    {
        _db = db;
        _categoryService = categoryService;
    }

    private IQueryable<Article> ArticlesWithCategory() =>
        _db.Articles.Include(a => a.Category);

    /// <summary>Filter by optional category and optional keyword (title or summary), newest first.</summary>
    public async Task<PageResponse<Article>> SearchAsync(int? categoryId, string? keyword, int page, int size)
    {
        page = Math.Max(page, 0);
        size = Math.Clamp(size, 1, MaxPageSize);

        var query = ArticlesWithCategory().AsNoTracking();
        if (categoryId.HasValue)
            query = query.Where(a => a.CategoryId == categoryId.Value);
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var q = keyword.Trim().ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(q) || (a.Summary != null && a.Summary.ToLower().Contains(q)));
        }

        int total = await query.CountAsync();
        var items = await query
            .OrderByDescending(a => a.PublishedAt)
            .Skip(page * size)
            .Take(size)
            .ToListAsync();

        int totalPages = (int)Math.Ceiling(total / (double)size);
        return new PageResponse<Article>(items, page, size, total, totalPages, page >= totalPages - 1);
    }

    public Task<List<Article>> GetFeaturedAsync() =>
        ArticlesWithCategory().AsNoTracking()
            .Where(a => a.Featured)
            .OrderByDescending(a => a.PublishedAt)
            .Take(5)
            .ToListAsync();

    public Task<List<Article>> GetTrendingAsync() =>
        ArticlesWithCategory().AsNoTracking()
            .OrderByDescending(a => a.Views)
            .ThenByDescending(a => a.PublishedAt)
            .Take(5)
            .ToListAsync();

    public async Task<Article> GetAsync(int id) =>
        await ArticlesWithCategory().FirstOrDefaultAsync(a => a.Id == id)
        ?? throw new NotFoundException($"Article not found with id {id}");

    /// <summary>Returns an article and counts one view.</summary>
    public async Task<Article> ReadAsync(int id)
    {
        var article = await GetAsync(id);
        article.Views++;
        await _db.SaveChangesAsync();
        return article;
    }

    public async Task<Article> CreateAsync(ArticleRequest request)
    {
        var article = new Article();
        await ApplyAsync(article, request);
        article.PublishedAt = DateTime.Now;
        article.UpdatedAt = article.PublishedAt;
        _db.Articles.Add(article);
        await _db.SaveChangesAsync();
        return article;
    }

    public async Task<Article> UpdateAsync(int id, ArticleRequest request)
    {
        var article = await GetAsync(id);
        await ApplyAsync(article, request);
        article.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();
        return article;
    }

    public async Task DeleteAsync(int id)
    {
        var article = await GetAsync(id);
        var comments = await _db.Comments.Where(c => c.ArticleId == id).ToListAsync();
        _db.Comments.RemoveRange(comments);
        _db.Articles.Remove(article);
        await _db.SaveChangesAsync();
    }

    private async Task ApplyAsync(Article article, ArticleRequest r)
    {
        article.Title = r.Title.Trim();
        article.Summary = r.Summary;
        article.Content = r.Content;
        article.ImageUrl = r.ImageUrl;
        article.Author = r.Author.Trim();
        article.Category = await _categoryService.GetAsync(r.CategoryId!.Value);
        article.Featured = r.Featured;
    }
}
