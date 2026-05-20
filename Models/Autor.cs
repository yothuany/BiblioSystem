namespace BiblioSystem.Models;

public class Autor
{
    public int IdAutor { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Biografia { get; set; }

    public ICollection<LivroAutor> LivroAutores { get; set; } = [];
}
