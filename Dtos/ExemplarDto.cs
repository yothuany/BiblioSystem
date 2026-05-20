namespace BiblioSystem.Dtos.Exemplar;

public record ExemplarCreateDto(string Codigo, int LivroIdLivro);
public record ExemplarUpdateDto(string Codigo, string Status);
public record ExemplarResponseDto(int IdExemplar, string Codigo, string Status, int LivroIdLivro, string TituloLivro);
