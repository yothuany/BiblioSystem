namespace BiblioSystem.Models;

public class Reserva
{
    public int IdReserva { get; set; }
    public DateOnly DataReserva { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string Status { get; set; } = "pendente"; // pendente | atendida | cancelada

    public int MembroIdMembro { get; set; }
    public Membro Membro { get; set; } = null!;

    public int LivroIdLivro { get; set; }
    public Livro Livro { get; set; } = null!;
}
