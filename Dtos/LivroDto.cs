namespace BiblioSystem.Dtos.Livro;

public record LivroCreateDto(
    string Titulo,
    string Isbn,
    int AnoPublicacao,
    string Editora,
    List<int> AutorIds,
    List<int> CategoriaIds
);

public record LivroUpdateDto(
    string Titulo,
    string Isbn,
    int AnoPublicacao,
    string Editora
);

public record LivroResponseDto(
    int IdLivro,
    string Titulo,
    string Isbn,
    int AnoPublicacao,
    string Editora,
    List<string> Autores,
    List<string> Categorias
);
