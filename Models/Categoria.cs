using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("categorias"), PrimaryKey(nameof(Id))]
    public class Categoria
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public required string Descricao { get; set; }

        public ICollection<Livro>? Livros { get; set; }
    }
}