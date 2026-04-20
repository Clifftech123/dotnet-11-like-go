using src.domain.category;
using src.service;

namespace src.handler;

class CategoryHandler(ICategoryService service)
{
    public async Task<IResult> ListCategories() =>
        Results.Ok(await service.GetAllAsync());

    public async Task<IResult> GetCategory(Guid id)
    {
        var category = await service.GetByIdAsync(id);
        return category is null ? Results.NotFound() : Results.Ok(category);
    }

    public async Task<IResult> CreateCategory(CreateCategoryDto request)
    {
        var category = await service.CreateAsync(request);
        return Results.Created($"/api/categories/{category.Id}", category);
    }

    public async Task<IResult> UpdateCategory(Guid id, UpdateCategoryDto request)
    {
        var category = await service.UpdateAsync(id, request);
        return category is null ? Results.NotFound() : Results.Ok(category);
    }

    public async Task<IResult> DeleteCategory(Guid id) =>
        await service.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
}
