using src.entity;

namespace src.repository
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> GetAllAsync(CancellationToken ct = default);
        Task<Blog?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Blog>> GetByAuthorAsync(Guid authorId, CancellationToken ct = default);
        Task AddAsync(Blog blog, CancellationToken ct = default);
        void Update(Blog blog);
        void Remove(Blog blog);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
