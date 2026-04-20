using src.entity;

namespace src.repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Category?> GetByNameAsync(string name, CancellationToken ct = default);
        Task AddAsync(Category category, CancellationToken ct = default);
        void Update(Category category);
        void Remove(Category category);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
