using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("Livro"), PrimaryKey(nameof(Id))]
    public class Livro
    {
        [Column("id_livro")]
        public int Id { get; set; }

        [Column("titulo")]
        public required string Titulo { get; set; }

        [Column("isbn")]
        public required string Isbn { get; set; }

        [Column("ano_publicacao")]
        public required int AnoPublicacao { get; set; }

        [Column("editora")]
        public string Editora { get; set; }


        // (N:N)
        public virtual ICollection<Autor>? Autores { get; set; } = new List<Autor>();

        public virtual ICollection<Categoria>? Categorias { get; set; } = new List<Categoria>();


        // (1:N)
        public virtual ICollection<Exemplar>? Exemplares { get; set; } = new List<Exemplar>();

        public virtual ICollection<Reserva>? Reservas { get; set; } = new List<Reserva>();
    }
}