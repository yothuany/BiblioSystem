namespace BiblioSystem.Models;

public class LivroAutor
{
    public int LivroIdLivro { get; set; }
    public Livro Livro { get; set; } = null!;

    public int AutorIdAutor { get; set; }
    public Autor Autor { get; set; } = null!;
}
