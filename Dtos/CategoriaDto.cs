using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class CategoriaDto
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [MinLength(
            3,
            ErrorMessage = "Obrigatório mínimo de 3 caracteres"
        )]
        public required string Nome { get; set; }

        public string? Descricao { get; set; }
    }
}