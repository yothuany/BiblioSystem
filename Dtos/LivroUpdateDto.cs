using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class LivroUpdateDto : LivroDto
    {
        [Required]
        public required string Situacao { get; set; }

        [Required]
        public DateOnly DataAtualizacao { get; set; }
    }
}