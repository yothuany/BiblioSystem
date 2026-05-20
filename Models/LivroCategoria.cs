namespace BiblioSystem.Models;

public class LivroCategoria
{
    public int LivroIdLivro { get; set; }
    public Livro Livro { get; set; } = null!;

    public int CategoriaIdCategoria { get; set; }
    public Categoria Categoria { get; set; } = null!;
}
