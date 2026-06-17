using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("membros"), PrimaryKey(nameof(Id))]
    public class Membro
    {
        public int Id { get; set; }

        public required string NomeCompleto { get; set; }

        public required string CPF { get; set; }

        public required string Email { get; set; }

        public required string Telefone { get; set; }

        public DateOnly DataCadastro { get; set; }

        public ICollection<Emprestimo>? Emprestimos { get; set; }

        public ICollection<Reserva>? Reservas { get; set; }
    }
}