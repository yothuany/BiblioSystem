namespace BiblioSystem.Models;

public class Emprestimo
{
    public int IdEmprestimo { get; set; }
    public DateOnly DataEmprestimo { get; set; }
    public DateOnly DataPrevistaDevolucao { get; set; }
    public DateOnly? DataDevolucao { get; set; }
    public decimal ValorMulta { get; set; } = 0;

    public int MembroIdMembro { get; set; }
    public Membro Membro { get; set; } = null!;

    public int ExemplarIdExemplar { get; set; }
    public Exemplar Exemplar { get; set; } = null!;
}
