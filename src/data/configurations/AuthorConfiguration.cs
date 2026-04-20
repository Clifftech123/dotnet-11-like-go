using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.entity;

namespace src.data.configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("authors");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(a => a.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .HasMaxLength(1000)
                .HasDefaultValue("");

            builder.Property(a => a.Email)
                .HasColumnName("email")
                .HasMaxLength(320)
                .IsRequired();

            builder.HasIndex(a => a.Email).IsUnique();

            builder.HasMany(a => a.Blogs)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
