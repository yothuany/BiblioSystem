using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("Autor"), PrimaryKey(nameof(Id))]
    public class Autor
    {
        [Column("id_autor")]
        public int Id { get; set; }

        [Column("nome")]
        public required string Nome { get; set; }

        [Column("biografia")]
        public required string Biografia { get; set; }

        public ICollection<Livro>? Livros { get; set; }
    }
}