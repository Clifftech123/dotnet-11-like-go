namespace src.entity
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";

        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}