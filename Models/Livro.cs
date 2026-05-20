namespace BiblioSystem.Models;

public class Livro
{
    public int IdLivro { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int AnoPublicacao { get; set; }
    public string Editora { get; set; } = string.Empty;

    public ICollection<LivroAutor> LivroAutores { get; set; } = [];
    public ICollection<LivroCategoria> LiveCategoria { get; set; } = [];
    public ICollection<Exemplar> Exemplares { get; set; } = [];
    public ICollection<Reserva> Reservas { get; set; } = [];
}
