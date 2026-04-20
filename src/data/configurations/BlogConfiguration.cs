using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.entity;

namespace src.data.configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("blogs");

            builder.HasKey(b => b.BlogId);

            builder.Property(b => b.BlogId)
                .HasColumnName("blog_id")
                .ValueGeneratedNever();

            builder.Property(b => b.Title)
                .HasColumnName("title")
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(b => b.Content)
                .HasColumnName("content")
                .IsRequired();

            builder.Property(b => b.AuthorId)
                .HasColumnName("author_id")
                .IsRequired();

            builder.HasIndex(b => b.AuthorId);
        }
    }
}
