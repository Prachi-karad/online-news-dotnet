using Microsoft.EntityFrameworkCore;
using OnlineNews.Api.Data;
using OnlineNews.Api.Dtos;
using OnlineNews.Api.Middleware;
using OnlineNews.Api.Models;

namespace OnlineNews.Api.Services;

public class CommentService
{
    private readonly AppDbContext _db;
    private readonly ArticleService _articleService;

    public CommentService(AppDbContext db, ArticleService articleService)
    {
        _db = db;
        _articleService = articleService;
    }

    public async Task<List<Comment>> GetForArticleAsync(int articleId)
    {
        await _articleService.GetAsync(articleId);
        return await _db.Comments.AsNoTracking()
            .Where(c => c.ArticleId == articleId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Comment> AddAsync(int articleId, CommentRequest request)
    {
        var article = await _articleService.GetAsync(articleId);
        var comment = new Comment
        {
            Article = article,
            Name = request.Name.Trim(),
            Text = request.Text.Trim(),
            CreatedAt = DateTime.Now
        };
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        return comment;
    }

    public async Task DeleteAsync(int id)
    {
        var comment = await _db.Comments.FindAsync(id)
                      ?? throw new NotFoundException($"Comment not found with id {id}");
        _db.Comments.Remove(comment);
        await _db.SaveChangesAsync();
    }
}
