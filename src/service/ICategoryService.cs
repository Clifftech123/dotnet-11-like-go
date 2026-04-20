using src.domain.category;

namespace src.service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<CategoryResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default);
        Task<CategoryResponseDto?> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
