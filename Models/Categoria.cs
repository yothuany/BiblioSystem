namespace BiblioSystem.Models;

public class Categoria
{
    public int IdCategoria { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public ICollection<LivroCategoria> LiveCategoria { get; set; } = [];
}
