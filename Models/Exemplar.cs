namespace BiblioSystem.Models;

public class Exemplar
{
    public int IdExemplar { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Status { get; set; } = "disponivel"; // disponivel | emprestado | reservado

    public int LivroIdLivro { get; set; }
    public Livro Livro { get; set; } = null!;

    public ICollection<Emprestimo> Emprestimos { get; set; } = [];
}
