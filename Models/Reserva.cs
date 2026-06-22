using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("Reserva"), PrimaryKey(nameof(Id))]
    public class Reserva
    {
        [Column("id_reserva")]
        public int Id { get; set; }

        [Column("data_reserva")]
        public required DateTime DataReserva { get; set; } 

        [Column("status")]
        public required string Status { get; set; }

        [Column("Membro_id_membro")]
        public required int MembroId { get; set; }

        [Column("Livro_id_livro")]
        public required int LivroId { get; set; }

        // (1:N)
        [ForeignKey(nameof(LivroId))]
        public virtual Livro? Livro { get; set; }

        [ForeignKey(nameof(MembroId))]
        public virtual Membro? Membro { get; set; }
    }
}