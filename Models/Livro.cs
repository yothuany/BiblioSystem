using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("livros"), PrimaryKey(nameof(Id))]
    public class Livro
    {
        public int Id { get; set; }

        public required string Titulo { get; set; }

        public required string ISBN { get; set; }

        public int AnoPublicacao { get; set; }

        public required string Editora { get; set; }

        public int CategoriaId { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public ICollection<Autor>? Autores { get; set; }

        public ICollection<Exemplar>? Exemplares { get; set; }
    }
}