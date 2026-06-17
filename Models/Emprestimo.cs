using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("emprestimos"), PrimaryKey(nameof(Id))]
    public class Emprestimo
    {
        public int Id { get; set; }

        public int MembroId { get; set; }

        public virtual Membro? Membro { get; set; }

        public int ExemplarId { get; set; }

        public virtual Exemplar? Exemplar { get; set; }

        public DateOnly DataEmprestimo { get; set; }

        public DateOnly DataPrevistaDevolucao { get; set; }

        public DateOnly? DataDevolucao { get; set; }

        public decimal Multa { get; set; }
    }
}