using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("usuarios"), PrimaryKey(nameof(Id))]
    public class Usuario
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public required string Email { get; set; }

        public required string Senha { get; set; }
    }
}