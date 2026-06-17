using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioSystem.Models
{
    [Table("reservas")]
    [PrimaryKey(nameof(Id))]
    public class Reserva
    {
        public int Id { get; set; }


        public int MembroId { get; set; }

        public virtual Membro? Membro { get; set; }


        public int LivroId { get; set; }

        public virtual Livro? Livro { get; set; }


        public DateTime DataReserva { get; set; }


        public string Status { get; set; }
            = "ativa";
    }
}