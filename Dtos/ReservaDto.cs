using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class ReservaDto
    {
        [Required(ErrorMessage = "O campo CpfMembro é obrigatório.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato válido (000.000.000-00).")]
        public required string CpfMembro { get; set; }

        [Required(ErrorMessage = "O campo IsbnLivro é obrigatório.")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "O ISBN deve ter entre 10 e 13 caracteres.")]
        public required string IsbnLivro { get; set; }
    }
}