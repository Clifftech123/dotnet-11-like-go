using Microsoft.EntityFrameworkCore;
using src.data;
using src.entity;

namespace src.repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default) =>
            await _db.Categories.AsNoTracking().ToListAsync(ct);

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<Category?> GetByNameAsync(string name, CancellationToken ct = default) =>
            await _db.Categories.FirstOrDefaultAsync(c => c.Name == name, ct);

        public async Task AddAsync(Category category, CancellationToken ct = default) =>
            await _db.Categories.AddAsync(category, ct);

        public void Update(Category category) => _db.Categories.Update(category);

        public void Remove(Category category) => _db.Categories.Remove(category);

        public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _db.SaveChangesAsync(ct);
    }
}
