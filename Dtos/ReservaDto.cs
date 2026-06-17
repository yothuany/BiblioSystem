using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class ReservaDto
    {
        [Required]
        public int MembroId { get; set; }


        [Required]
        public int LivroId { get; set; }
    }
}