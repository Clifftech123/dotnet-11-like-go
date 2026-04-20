using src.domain.blog;
using src.service;

namespace src.handler;

class BlogHandler(IBlogService service)
{
    public async Task<IResult> ListBlogs() =>
        Results.Ok(await service.GetAllAsync());

    public async Task<IResult> ListBlogsByAuthor(Guid authorId) =>
        Results.Ok(await service.GetByAuthorAsync(authorId));

    public async Task<IResult> GetBlog(Guid id)
    {
        var blog = await service.GetByIdAsync(id);
        return blog is null ? Results.NotFound() : Results.Ok(blog);
    }

    public async Task<IResult> CreateBlog(CreateBlogDto request)
    {
        var blog = await service.CreateAsync(request);
        return Results.Created($"/api/blogs/{blog.BlogId}", blog);
    }

    public async Task<IResult> UpdateBlog(Guid id, UpdateBlogDto request)
    {
        var blog = await service.UpdateAsync(id, request);
        return blog is null ? Results.NotFound() : Results.Ok(blog);
    }

    public async Task<IResult> DeleteBlog(Guid id) =>
        await service.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
}
