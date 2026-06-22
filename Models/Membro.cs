using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("Membro"), PrimaryKey(nameof(Id))]
    public class Membro
    {
        [Column("id_membro")]
        public int Id { get; set; }

        [Column("nome")]
        public required string Nome { get; set; }

        [Column("email")]
        public required string Email { get; set; }

        [Column("telefone")]
        public required string Telefone { get; set; } 

        [Column("cpf")]
        public required string Cpf { get; set; }

        // (1:N)
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}