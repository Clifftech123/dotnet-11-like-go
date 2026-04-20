using src.domain.author;

namespace src.service
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<AuthorResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<AuthorResponseDto> CreateAsync(CreateAuthorDto dto, CancellationToken ct = default);
        Task<AuthorResponseDto?> UpdateAsync(Guid id, UpdateAuthorDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
