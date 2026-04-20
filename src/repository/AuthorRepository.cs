using Microsoft.EntityFrameworkCore;
using src.data;
using src.entity;

namespace src.repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _db;

        public AuthorRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<IEnumerable<Author>> GetAllAsync(CancellationToken ct = default) =>
            Task.FromResult<IEnumerable<Author>>(_db.Authors.AsNoTracking().ToList());

        public async Task<Author?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _db.Authors.FirstOrDefaultAsync(a => a.Id == id, ct);

        public async Task<Author?> GetByEmailAsync(string email, CancellationToken ct = default) =>
            await _db.Authors.FirstOrDefaultAsync(a => a.Email == email, ct);

        public async Task AddAsync(Author author, CancellationToken ct = default) =>
            await _db.Authors.AddAsync(author, ct);

        public void Update(Author author) => _db.Authors.Update(author);

        public void Remove(Author author) => _db.Authors.Remove(author);

        public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _db.SaveChangesAsync(ct);
    }
}
