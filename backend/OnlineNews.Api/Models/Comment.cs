using System.Text.Json.Serialization;

namespace OnlineNews.Api.Models;

public class Comment
{
    public int Id { get; set; }

    public int ArticleId { get; set; }

    [JsonIgnore]
    public Article? Article { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
