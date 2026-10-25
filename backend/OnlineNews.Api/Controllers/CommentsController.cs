using Microsoft.AspNetCore.Mvc;
using OnlineNews.Api.Services;

namespace OnlineNews.Api.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentsController(CommentService commentService) => _commentService = commentService;

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _commentService.DeleteAsync(id);
        return NoContent();
    }
}
