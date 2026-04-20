namespace src.domain.blog
{
    public record CreateBlogDto(string Title, string Content, Guid AuthorId);
}
