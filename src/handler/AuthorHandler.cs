using src.domain.author;
using src.service;

namespace src.handler;

class AuthorHandler(IAuthorService service)
{
    public async Task<IResult> ListAuthors() =>
        Results.Ok(await service.GetAllAsync());

    public async Task<IResult> GetAuthor(Guid id)
    {
        var author = await service.GetByIdAsync(id);
        return author is null ? Results.NotFound() : Results.Ok(author);
    }

    public async Task<IResult> CreateAuthor(CreateAuthorDto request)
    {
        var author = await service.CreateAsync(request);
        return Results.Created($"/api/authors/{author.Id}", author);
    }

    public async Task<IResult> UpdateAuthor(Guid id, UpdateAuthorDto request)
    {
        var author = await service.UpdateAsync(id, request);
        return author is null ? Results.NotFound() : Results.Ok(author);
    }

    public async Task<IResult> DeleteAuthor(Guid id) =>
        await service.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
}
