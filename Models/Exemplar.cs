using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("Exemplar"), PrimaryKey(nameof(Id))]
    public class Exemplar
    {
        [Column("id_exemplar")]
        public int Id { get; set; }

        [Column("codigo")]
        public required string Codigo { get; set; }

        [Column("status")]
        public required string Status { get; set; }

        [Column("Livro_id_livro")]
        public int LivroId { get; set; }

        // (1:N)
        [ForeignKey(nameof(LivroId))]
        public Livro? Livro { get; set; }
    }
}