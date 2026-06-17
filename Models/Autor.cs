using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("autores"), PrimaryKey(nameof(Id))]
    public class Autor
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public required string Biografia { get; set; }

        public ICollection<Livro>? Livros { get; set; }
    }
}