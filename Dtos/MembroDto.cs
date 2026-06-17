using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class MembroDto
    {
        [Required]
        public required string NomeCompleto { get; set; }

        [Required]
        public required string CPF { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Telefone { get; set; }
    }
}