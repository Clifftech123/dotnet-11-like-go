

namespace src.entity
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set;}
        public string Description { get; set;} = "";
        public string Email { get; set;} = "";

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();

        public Author(string name, string description, string email)
        {
            Name = name;
            Description = description;
            Email = email;
        }
        
    }
}