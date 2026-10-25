using Microsoft.AspNetCore.Mvc;
using OnlineNews.Api.Dtos;
using OnlineNews.Api.Models;
using OnlineNews.Api.Services;

namespace OnlineNews.Api.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticlesController : ControllerBase
{
    private readonly ArticleService _articleService;
    private readonly CommentService _commentService;

    public ArticlesController(ArticleService articleService, CommentService commentService)
    {
        _articleService = articleService;
        _commentService = commentService;
    }

    /// <summary>GET /api/articles?categoryId=1&amp;search=india&amp;page=0&amp;size=9</summary>
    [HttpGet]
    public Task<PageResponse<Article>> Search([FromQuery] int? categoryId, [FromQuery] string? search,
                                              [FromQuery] int page = 0, [FromQuery] int size = 9) =>
        _articleService.SearchAsync(categoryId, search, page, size);

    [HttpGet("featured")]
    public Task<List<Article>> Featured() => _articleService.GetFeaturedAsync();

    [HttpGet("trending")]
    public Task<List<Article>> Trending() => _articleService.GetTrendingAsync();

    /// <summary>Reading an article increases its view count. Pass ?count=false to skip that (e.g. admin edit).</summary>
    [HttpGet("{id:int}")]
    public Task<Article> Get(int id, [FromQuery] bool count = true) =>
        count ? _articleService.ReadAsync(id) : _articleService.GetAsync(id);

    [HttpPost]
    public async Task<ActionResult<Article>> Create(ArticleRequest request)
    {
        var article = await _articleService.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = article.Id }, article);
    }

    [HttpPut("{id:int}")]
    public Task<Article> Update(int id, ArticleRequest request) => _articleService.UpdateAsync(id, request);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _articleService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id:int}/comments")]
    public Task<List<Comment>> Comments(int id) => _commentService.GetForArticleAsync(id);

    [HttpPost("{id:int}/comments")]
    public async Task<ActionResult<Comment>> AddComment(int id, CommentRequest request)
    {
        var comment = await _commentService.AddAsync(id, request);
        return StatusCode(StatusCodes.Status201Created, comment);
    }
}
