namespace BiblioSystem.Dtos.Autor;

public record AutorCreateDto(string Nome, string? Biografia);
public record AutorUpdateDto(string Nome, string? Biografia);
public record AutorResponseDto(int IdAutor, string Nome, string? Biografia);
