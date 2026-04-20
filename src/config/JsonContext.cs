using System.Text.Json.Serialization;
using src.domain.author;
using src.domain.blog;
using src.domain.category;

namespace src.config;

[JsonSerializable(typeof(CreateAuthorDto))]
[JsonSerializable(typeof(UpdateAuthorDto))]
[JsonSerializable(typeof(AuthorResponseDto))]
[JsonSerializable(typeof(IEnumerable<AuthorResponseDto>))]
[JsonSerializable(typeof(CreateBlogDto))]
[JsonSerializable(typeof(UpdateBlogDto))]
[JsonSerializable(typeof(BlogResponseDto))]
[JsonSerializable(typeof(IEnumerable<BlogResponseDto>))]
[JsonSerializable(typeof(CreateCategoryDto))]
[JsonSerializable(typeof(UpdateCategoryDto))]
[JsonSerializable(typeof(CategoryResponseDto))]
[JsonSerializable(typeof(IEnumerable<CategoryResponseDto>))]
[JsonSerializable(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
