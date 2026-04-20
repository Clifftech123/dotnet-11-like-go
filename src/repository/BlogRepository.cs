using Microsoft.EntityFrameworkCore;
using src.data;
using src.entity;

namespace src.repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _db;

        public BlogRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Blog>> GetAllAsync(CancellationToken ct = default) =>
            await _db.Blogs.AsNoTracking().ToListAsync(ct);

        public async Task<Blog?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _db.Blogs.FirstOrDefaultAsync(b => b.BlogId == id, ct);

        public async Task<IEnumerable<Blog>> GetByAuthorAsync(Guid authorId, CancellationToken ct = default) =>
            await _db.Blogs.AsNoTracking().Where(b => b.AuthorId == authorId).ToListAsync(ct);

        public async Task AddAsync(Blog blog, CancellationToken ct = default) =>
            await _db.Blogs.AddAsync(blog, ct);

        public void Update(Blog blog) => _db.Blogs.Update(blog);

        public void Remove(Blog blog) => _db.Blogs.Remove(blog);

        public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _db.SaveChangesAsync(ct);
    }
}
