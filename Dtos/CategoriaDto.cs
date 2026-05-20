namespace BiblioSystem.Dtos.Categoria;

public record CategoriaCreateDto(string Nome, string? Descricao);
public record CategoriaUpdateDto(string Nome, string? Descricao);
public record CategoriaResponseDto(int IdCategoria, string Nome, string? Descricao);
