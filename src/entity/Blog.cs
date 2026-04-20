
namespace src.entity
{
    public class Blog
    {
        public Guid BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } 
        
    }
}