using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BiblioSystem.Models
{
    [Table("livros"), PrimaryKey(nameof(Id))]
    public class Livro
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("titulo")]
        public required string Titulo { get; set; }

        [Column("autor")]
        public required string Autor { get; set; }

        [Column("ano")]
        public required int Ano { get; set; }

        public string Situacao { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        [JsonIgnore]
        [Column("categoria_id")]
        public int? CategoriaId { get; set; }

        public virtual Categoria? Categoria { get; set; }
    }
}