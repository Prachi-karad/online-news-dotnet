using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Api.Dtos;

public class ArticleRequest
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Summary can be at most 500 characters")]
    public string? Summary { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Author is required")]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required")]
    public int? CategoryId { get; set; }

    public bool Featured { get; set; }
}

public class CommentRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Comment cannot be empty")]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;
}

/// <summary>Stable JSON shape for paginated results.</summary>
public record PageResponse<T>(
    List<T> Content,
    int Page,
    int Size,
    int TotalElements,
    int TotalPages,
    bool Last);
