using src.domain.category;
using src.entity;
using src.repository;

namespace src.service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
            var items = await _repo.GetAllAsync(ct);
            return items.Select(ToResponse);
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var item = await _repo.GetByIdAsync(id, ct);
            return item is null ? null : ToResponse(item);
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default)
        {
            var category = new Category(dto.Name, dto.Description)
            {
                Id = Guid.NewGuid()
            };
            await _repo.AddAsync(category, ct);
            await _repo.SaveChangesAsync(ct);
            return ToResponse(category);
        }

        public async Task<CategoryResponseDto?> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken ct = default)
        {
            var category = await _repo.GetByIdAsync(id, ct);
            if (category is null) return null;

            category.Name = dto.Name;
            category.Description = dto.Description;

            _repo.Update(category);
            await _repo.SaveChangesAsync(ct);
            return ToResponse(category);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var category = await _repo.GetByIdAsync(id, ct);
            if (category is null) return false;

            _repo.Remove(category);
            await _repo.SaveChangesAsync(ct);
            return true;
        }

        private static CategoryResponseDto ToResponse(Category c) =>
            new(c.Id, c.Name, c.Description);
    }
}
