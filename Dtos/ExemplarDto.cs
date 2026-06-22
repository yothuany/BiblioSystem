using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class ExemplarDto
    {
        [Required(ErrorMessage = "O código do exemplar é obrigatório.")]
        public required string Codigo { get; set; }

        [Required(ErrorMessage = "O campo IsbnLivro é obrigatório.")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "O ISBN deve ter entre 10 e 13 caracteres.")]
        public required string IsbnLivro { get; set; }
    }
}