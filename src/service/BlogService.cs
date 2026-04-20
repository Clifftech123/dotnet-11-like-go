using src.domain.blog;
using src.entity;
using src.repository;

namespace src.service
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _repo;
        private readonly IAuthorRepository _authorRepo;

        public BlogService(IBlogRepository repo, IAuthorRepository authorRepo)
        {
            _repo = repo;
            _authorRepo = authorRepo;
        }

        public async Task<IEnumerable<BlogResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
            var blogs = await _repo.GetAllAsync(ct);
            return blogs.Select(ToResponse);
        }

        public async Task<BlogResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var blog = await _repo.GetByIdAsync(id, ct);
            return blog is null ? null : ToResponse(blog);
        }

        public async Task<IEnumerable<BlogResponseDto>> GetByAuthorAsync(Guid authorId, CancellationToken ct = default)
        {
            var blogs = await _repo.GetByAuthorAsync(authorId, ct);
            return blogs.Select(ToResponse);
        }

        public async Task<BlogResponseDto> CreateAsync(CreateBlogDto dto, CancellationToken ct = default)
        {
            var author = await _authorRepo.GetByIdAsync(dto.AuthorId, ct)
                ?? throw new InvalidOperationException($"Author {dto.AuthorId} not found");

            var blog = new Blog
            {
                BlogId = Guid.NewGuid(),
                Title = dto.Title,
                Content = dto.Content,
                AuthorId = author.Id
            };

            await _repo.AddAsync(blog, ct);
            await _repo.SaveChangesAsync(ct);
            return ToResponse(blog);
        }

        public async Task<BlogResponseDto?> UpdateAsync(Guid id, UpdateBlogDto dto, CancellationToken ct = default)
        {
            var blog = await _repo.GetByIdAsync(id, ct);
            if (blog is null) return null;

            blog.Title = dto.Title;
            blog.Content = dto.Content;

            _repo.Update(blog);
            await _repo.SaveChangesAsync(ct);
            return ToResponse(blog);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var blog = await _repo.GetByIdAsync(id, ct);
            if (blog is null) return false;

            _repo.Remove(blog);
            await _repo.SaveChangesAsync(ct);
            return true;
        }

        private static BlogResponseDto ToResponse(Blog b) =>
            new(b.BlogId, b.Title, b.Content, b.AuthorId);
    }
}
