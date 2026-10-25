# 📰 NewsHub – Online News Application (.NET)

A full-stack news portal built with **ASP.NET Core Web API (C#)**, **Entity Framework Core**, **SQL Server** and **React.js**.
Readers can browse the latest headlines by category, search articles, read full stories and comment.
Editors use the admin panel to publish, edit and delete articles and manage categories.

## ✨ Features

**Reader**
- Home page with featured stories, latest news grid and a 🔥 trending sidebar (most viewed)
- Browse news by category (India, Technology, Sports, Business, Entertainment…)
- Live search on headline and summary
- Pagination
- Article page with reading time, view counter, share link and comments

**Admin / Editor**
- Publish, edit and delete articles
- Mark articles as *featured* for the home page
- Add and delete categories
- Article list with views and publish date

**Backend**
- Layered architecture: Controllers → Services → EF Core `DbContext`
- Pagination, sorting and dynamic LINQ search (category + keyword)
- View counter on every article read
- Data Annotations validation with clear field-level error messages
- Global error-handling middleware with consistent JSON error responses
- Swagger UI for testing the API
- Sample categories and articles inserted on first start
- In-memory database option – run without SQL Server
- CORS configured for the React dev server

## 🛠 Tech Stack

| Layer     | Technology                                                    |
|-----------|---------------------------------------------------------------|
| Backend   | C#, .NET 8, ASP.NET Core Web API, Entity Framework Core 8      |
| Database  | SQL Server (EF Core In-Memory option for quick testing)       |
| Frontend  | React 18, React Router 6, Axios, Vite                         |
| Tools     | Swagger, Git, GitHub Actions                                  |

## 📁 Project Structure

```
online-news-dotnet/
├── backend/OnlineNews.Api/             ASP.NET Core Web API
│   ├── Controllers/                    ArticlesController, CategoriesController, CommentsController
│   ├── Services/                       Business logic
│   ├── Models/                         Category, Article, Comment entities
│   ├── Data/                           AppDbContext + sample data seeder
│   ├── Dtos/                           Request / response objects
│   ├── Middleware/                     Error handling + custom exceptions
│   ├── Program.cs                      App startup & dependency injection
│   └── appsettings.json                Connection string & settings
└── frontend/                           React application (Vite)
    └── src/
        ├── pages/                      Home, ArticlePage, Admin
        ├── components/                 Navbar, ArticleCard, Pagination
        └── api.js                      Axios API client
```

## 🗄 Database Design

```
Categories (Id, Name UNIQUE, Description)
    1
    │
    *
Articles (Id, Title, Summary, Content, ImageUrl, Author, CategoryId FK,
          Featured, Views, PublishedAt, UpdatedAt)
    1
    │
    *
Comments (Id, ArticleId FK, Name, Text, CreatedAt)
```

## 🔌 REST API

| Method | Endpoint                          | Description                                          |
|--------|-----------------------------------|------------------------------------------------------|
| GET    | `/api/categories`                 | List categories                                      |
| POST   | `/api/categories`                 | Add category                                         |
| DELETE | `/api/categories/{id}`            | Delete category (only if it has no articles)         |
| GET    | `/api/articles`                   | Paginated list (`?categoryId=&search=&page=&size=`)  |
| GET    | `/api/articles/featured`          | Featured articles                                    |
| GET    | `/api/articles/trending`          | Top 5 most viewed                                    |
| GET    | `/api/articles/{id}`              | Article details (+1 view; `?count=false` to skip)    |
| POST   | `/api/articles`                   | Publish article                                      |
| PUT    | `/api/articles/{id}`              | Update article                                       |
| DELETE | `/api/articles/{id}`              | Delete article and its comments                      |
| GET    | `/api/articles/{id}/comments`     | Comments of an article                               |
| POST   | `/api/articles/{id}/comments`     | Add comment                                          |
| DELETE | `/api/comments/{id}`              | Delete comment                                       |

Example request:

```json
POST /api/articles
{
  "title": "Pune emerges as a top hub for .NET developers",
  "summary": "IT companies are expanding hiring for ASP.NET Core roles.",
  "content": "First paragraph...\n\nSecond paragraph...",
  "imageUrl": "https://picsum.photos/seed/code/800/450",
  "author": "Tech Desk",
  "categoryId": 2,
  "featured": true
}
```

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Node.js 18+
- SQL Server / SQL Server Express / LocalDB (optional – see in-memory option)

### 1. Backend

```bash
cd backend/OnlineNews.Api
# update ConnectionStrings:DefaultConnection in appsettings.json if needed
dotnet run
```

The database `NewsAppDb` and its tables are created automatically on first run.
API runs on **http://localhost:8080**, Swagger UI on **http://localhost:8080/swagger**.

No SQL Server? Set `"UseInMemoryDatabase": true` in `appsettings.json`, or run:

```bash
dotnet run --UseInMemoryDatabase=true
```

Using LocalDB (comes with Visual Studio)? Use this connection string:
`Server=(localdb)\\MSSQLLocalDB;Database=NewsAppDb;Trusted_Connection=True;TrustServerCertificate=True`

### 2. Frontend

```bash
cd frontend
npm install
npm run dev
```

Open **http://localhost:5173**.

## 👩‍💻 Author

**Prachi Karad** – .NET Full Stack Developer
