using OnlineNews.Api.Models;

namespace OnlineNews.Api.Data;

/// <summary>Inserts sample categories and articles on first start (only when the database is empty).</summary>
public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Categories.Any()) return;

        var india = new Category { Name = "India", Description = "National news" };
        var tech = new Category { Name = "Technology", Description = "Gadgets, software and startups" };
        var sports = new Category { Name = "Sports", Description = "Cricket, football and more" };
        var business = new Category { Name = "Business", Description = "Markets and economy" };
        var entertainment = new Category { Name = "Entertainment", Description = "Movies, music and OTT" };
        db.Categories.AddRange(india, tech, sports, business, entertainment);

        var now = DateTime.Now;
        db.Articles.AddRange(
            CreateArticle("Monsoon brings relief to Marathwada farmers",
                "Good rainfall this season has filled reservoirs across Nanded, Latur and Beed districts.",
                "Farmers in the Marathwada region are hopeful after steady monsoon rains filled several major reservoirs.\n\n" +
                "Officials said water storage is significantly higher than last year, which should help the rabi crop season.\n\n" +
                "Agriculture experts advise farmers to plan sowing based on soil moisture and local forecasts.",
                "Staff Reporter", india, true, "rain", now.AddHours(-1)),
            CreateArticle("Pune emerges as a top hub for .NET developers",
                "IT companies in Pune are expanding hiring for ASP.NET Core and cloud roles.",
                "Recruiters say demand for backend developers skilled in C#, ASP.NET Core and Azure remains strong.\n\n" +
                "Freshers with solid project portfolios on GitHub are getting more interview calls, according to placement trainers.\n\n" +
                "Skills in REST API design, SQL Server and React are frequently listed together in full stack job descriptions.",
                "Tech Desk", tech, true, "code", now.AddHours(-2)),
            CreateArticle("India clinch thrilling last-over win in T20 series opener",
                "A late batting surge sealed a four-wicket victory in front of a packed stadium.",
                "Chasing a target of 182, India needed 14 runs off the final over and got there with a ball to spare.\n\n" +
                "The middle order rebuilt the innings after early wickets, and the finisher struck two sixes in the last over.\n\n" +
                "The second match of the series will be played on Sunday.",
                "Sports Desk", sports, true, "cricket", now.AddHours(-3)),
            CreateArticle("Small businesses turn to UPI for everyday payments",
                "Digital payments continue to grow in tier-2 and tier-3 cities.",
                "Shop owners report that most customers now prefer paying with UPI for small purchases.\n\n" +
                "Bankers say this shift is helping small businesses build a transaction history for loans.",
                "Business Bureau", business, false, "market", now.AddHours(-5)),
            CreateArticle("Regional cinema shines at the box office this festive season",
                "Marathi and other regional films are drawing large audiences in theaters.",
                "Distributors say strong word of mouth has kept regional films running for weeks.\n\n" +
                "Several films are also getting OTT releases soon after their theatrical run.",
                "Entertainment Desk", entertainment, false, "cinema", now.AddHours(-8)),
            CreateArticle("New AI tools help students learn programming faster",
                "Educators are adding AI assistants to coding courses.",
                "Colleges are experimenting with AI-powered tools that explain errors and suggest fixes.\n\n" +
                "Teachers say the key is to use them for understanding concepts, not just copying answers.",
                "Tech Desk", tech, false, "laptop", now.AddHours(-12)),
            CreateArticle("Football league kicks off with record attendance",
                "Opening weekend saw packed stands across all venues.",
                "Organisers reported record ticket sales for the opening weekend of the new season.\n\n" +
                "Young Indian players impressed in several matches.",
                "Sports Desk", sports, false, "football", now.AddDays(-1)));

        db.SaveChanges();
    }

    private static Article CreateArticle(string title, string summary, string content, string author,
        Category category, bool featured, string imageSeed, DateTime publishedAt) => new()
    {
        Title = title,
        Summary = summary,
        Content = content,
        Author = author,
        Category = category,
        Featured = featured,
        ImageUrl = $"https://picsum.photos/seed/{imageSeed}/800/450",
        PublishedAt = publishedAt,
        UpdatedAt = publishedAt
    };
}
