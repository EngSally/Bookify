

namespace Bookify.Domain;

public record BookDto(
 int Id,
 string Title,
 string? ImageThumbnailUrl,
 string Author
);

