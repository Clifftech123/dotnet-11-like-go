using src.domain.blog;

namespace src.service
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<BlogResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<BlogResponseDto>> GetByAuthorAsync(Guid authorId, CancellationToken ct = default);
        Task<BlogResponseDto> CreateAsync(CreateBlogDto dto, CancellationToken ct = default);
        Task<BlogResponseDto?> UpdateAsync(Guid id, UpdateBlogDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
