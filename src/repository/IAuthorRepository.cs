using src.entity;

namespace src.repository
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync(CancellationToken ct = default);
        Task<Author?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Author?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task AddAsync(Author author, CancellationToken ct = default);
        void Update(Author author);
        void Remove(Author author);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
