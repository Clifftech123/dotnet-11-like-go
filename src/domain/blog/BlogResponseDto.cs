namespace src.domain.blog
{
    public record BlogResponseDto(Guid BlogId, string Title, string Content, Guid AuthorId);
}
