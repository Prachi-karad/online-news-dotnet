using Microsoft.EntityFrameworkCore;
using OnlineNews.Api.Models;

namespace OnlineNews.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();

        modelBuilder.Entity<Article>(e =>
        {
            e.Property(a => a.Title).HasMaxLength(255).IsRequired();
            e.Property(a => a.Summary).HasMaxLength(500);
            e.Property(a => a.Author).HasMaxLength(100).IsRequired();
            e.HasIndex(a => a.PublishedAt);
            e.HasOne(a => a.Category).WithMany().HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Comment>(e =>
        {
            e.Property(c => c.Name).HasMaxLength(100).IsRequired();
            e.Property(c => c.Text).HasMaxLength(1000).IsRequired();
            e.HasOne(c => c.Article).WithMany().HasForeignKey(c => c.ArticleId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
