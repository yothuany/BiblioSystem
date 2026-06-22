using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("Categoria"), PrimaryKey(nameof(Id))]
    public class Categoria
    {
        [Column("id_categoria")]
        public int Id { get; set; }

        [Column("nome")]
        public required string Nome { get; set; } = string.Empty;

        [Column("descricao")]
        public string? Descricao { get; set; }

        public virtual ICollection<Livro> Livro { get; set; } = new List<Livro>();
    }
}