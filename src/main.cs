#:property TargetFramework=net11.0
#:property LangVersion=preview
#:property ExperimentalFileBasedProgramEnableIncludeDirective=true
#:property ExperimentalFileBasedProgramEnableTransitiveDirectives=true
#:property PublishAot=false
#:property IsAotCompatible=false

#:include ../packages.cs
#:include ../includes.cs




using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using src.config;
using src.data;
using src.domain.author;
using src.domain.blog;
using src.domain.category;
using src.handler;
using src.middleware;
using src.repository;
using src.service;

// Curious why I kept Program/Main? Modern C# lets you drop them (top-level statements),
// but I wanted a real entry point. Python has none and gets flak for it; Go has one and
// I wanted to mimic that. Also, no thanks to looking like a Node.js script.
class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddAppSettings();
        var connectionString = builder.Configuration.GetPostgresConnectionString();

        // dependency injection things
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
        builder.Services.ConfigureHttpJsonOptions(opt =>
            opt.SerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default));

        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddScoped<IAuthorService, AuthorService>();
        builder.Services.AddScoped<AuthorHandler>();

        builder.Services.AddScoped<IBlogRepository, BlogRepository>();
        builder.Services.AddScoped<IBlogService, BlogService>();
        builder.Services.AddScoped<BlogHandler>();

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<CategoryHandler>();

        builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddOpenApi();

    

        var app = builder.Build();

        // ensure schema exists (dev-only; real migrations come later)
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.EnsureCreatedAsync();
        }

        // middleware things
        app.UseExceptionHandler();
        app.UseMiddleware<RequestLogger>();
        app.MapOpenApi();
        app.MapScalarApiReference("/docs", options =>
        {
            options.WithTitle("dotnet-11-like-go")
                   .WithTheme(ScalarTheme.Mars)
                   .WithDefaultHttpClient(ScalarTarget.Shell, ScalarClient.Curl);
        });

        // endpoint things
        var authors = app.MapGroup("/api/authors").WithTags("authors");

        authors.MapGet(   "/",          (AuthorHandler h)                                      => h.ListAuthors());
        authors.MapGet(   "/{id:guid}", (Guid id, AuthorHandler h)                             => h.GetAuthor(id));
        authors.MapPost(  "/",          (CreateAuthorDto request, AuthorHandler h)             => h.CreateAuthor(request));
        authors.MapPut(   "/{id:guid}", (Guid id, UpdateAuthorDto request, AuthorHandler h)    => h.UpdateAuthor(id, request));
        authors.MapDelete("/{id:guid}", (Guid id, AuthorHandler h)                             => h.DeleteAuthor(id));

        var blogs = app.MapGroup("/api/blogs").WithTags("blogs");

        blogs.MapGet(   "/",                         (BlogHandler h)                                  => h.ListBlogs());
        blogs.MapGet(   "/{id:guid}",                (Guid id, BlogHandler h)                         => h.GetBlog(id));
        blogs.MapGet(   "/by-author/{authorId:guid}",(Guid authorId, BlogHandler h)                   => h.ListBlogsByAuthor(authorId));
        blogs.MapPost(  "/",                         (CreateBlogDto request, BlogHandler h)           => h.CreateBlog(request));
        blogs.MapPut(   "/{id:guid}",                (Guid id, UpdateBlogDto request, BlogHandler h)  => h.UpdateBlog(id, request));
        blogs.MapDelete("/{id:guid}",                (Guid id, BlogHandler h)                         => h.DeleteBlog(id));

        var categories = app.MapGroup("/api/categories").WithTags("categories");

        categories.MapGet(   "/",          (CategoryHandler h)                                         => h.ListCategories());
        categories.MapGet(   "/{id:guid}", (Guid id, CategoryHandler h)                                => h.GetCategory(id));
        categories.MapPost(  "/",          (CreateCategoryDto request, CategoryHandler h)              => h.CreateCategory(request));
        categories.MapPut(   "/{id:guid}", (Guid id, UpdateCategoryDto request, CategoryHandler h)     => h.UpdateCategory(id, request));
        categories.MapDelete("/{id:guid}", (Guid id, CategoryHandler h)                                => h.DeleteCategory(id));

        // let's start our C#-go-frankenstein :D
        await app.RunAsync();
    }
}