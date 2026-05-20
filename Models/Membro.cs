namespace BiblioSystem.Models;

public class Membro
{
    public int IdMembro { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateOnly DataCadastro { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public ICollection<Emprestimo> Emprestimos { get; set; } = [];
    public ICollection<Reserva> Reservas { get; set; } = [];
    public Usuario? Usuario { get; set; }
}
