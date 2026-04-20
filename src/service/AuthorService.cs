using src.domain.author;
using src.entity;
using src.repository;

namespace src.service
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repo;

        public AuthorService(IAuthorRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<AuthorResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
            var authors = await _repo.GetAllAsync(ct);
            return authors.Select(ToResponse);
        }

        public async Task<AuthorResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var author = await _repo.GetByIdAsync(id, ct);
            return author is null ? null : ToResponse(author);
        }

        public async Task<AuthorResponseDto> CreateAsync(CreateAuthorDto dto, CancellationToken ct = default)
        {
            var author = new Author(dto.Name, dto.Description, dto.Email)
            {
                Id = Guid.NewGuid()
            };
            await _repo.AddAsync(author, ct);
            await _repo.SaveChangesAsync(ct);
            return ToResponse(author);
        }

        public async Task<AuthorResponseDto?> UpdateAsync(Guid id, UpdateAuthorDto dto, CancellationToken ct = default)
        {
            var author = await _repo.GetByIdAsync(id, ct);
            if (author is null) return null;

            author.Name = dto.Name;
            author.Description = dto.Description;
            author.Email = dto.Email;

            _repo.Update(author);
            await _repo.SaveChangesAsync(ct);
            return ToResponse(author);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var author = await _repo.GetByIdAsync(id, ct);
            if (author is null) return false;

            _repo.Remove(author);
            await _repo.SaveChangesAsync(ct);
            return true;
        }

        private static AuthorResponseDto ToResponse(Author a) =>
            new(a.Id, a.Name, a.Description, a.Email);
    }
}
