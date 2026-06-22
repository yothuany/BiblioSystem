using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("Emprestimo"), PrimaryKey(nameof(Id))]
    public class Emprestimo
    {
        [Column("id_emprestimo")]
        public int Id { get; set; }

        [Column("data_emprestimo")]
        public required DateTime DataEmprestimo { get; set; }

        [Column("data_prevista_devolucao")]
        public required DateTime DataPrevistaDevolucao { get; set; }

        [Column("data_devolucao")]
        public DateTime? DataDevolucao { get; set; }

        [Column("valor_multa")]
        public decimal ValorMulta { get; set; } = 0.00m;

        [Column("status")]
        public required string Status { get; set; }

        [Column("Membro_id_membro")]
        public required int MembroId { get; set; }

        [Column("Exemplar_id_exemplar")]
        public required int ExemplarId { get; set; }

        [ForeignKey(nameof(MembroId))]
        public virtual Membro? Membro { get; set; }

        [ForeignKey(nameof(ExemplarId))]
        public virtual Exemplar? Exemplar { get; set; }
    }
}