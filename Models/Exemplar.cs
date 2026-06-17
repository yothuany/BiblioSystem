using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("exemplares"), PrimaryKey(nameof(Id))]
    public class Exemplar
    {
        public int Id { get; set; }

        public required string CodigoExemplar { get; set; }

        public required string Status { get; set; }

        public int LivroId { get; set; }

        public virtual Livro? Livro { get; set; }
    }
}