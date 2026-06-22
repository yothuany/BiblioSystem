using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("Usuario"), PrimaryKey(nameof(Id))]
    public class Usuario
    {
        [Column("id_usuario")]
        public int Id { get; set; }

        [Column("nome")]
        public required string Nome { get; set; }

        [Column("email")]
        public required string Email { get; set; }

        [Column("senha")]
        public required string Senha { get; set; }

        [Column("perfil")]
        public required string Perfil { get; set; } 
    }
}