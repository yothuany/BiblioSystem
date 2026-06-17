using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class ExemplarDto
    {
        [Required]
        public required string CodigoExemplar { get; set; }

        [Required]
        public int LivroId { get; set; }
    }
}