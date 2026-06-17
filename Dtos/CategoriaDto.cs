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

        [Required(
            ErrorMessage = "O campo Descrição é obrigatório"
        )]
        [MinLength(
            5,
            ErrorMessage = "Obrigatório mínimo de 5 caracteres"
        )]
        public required string Descricao { get; set; }
    }
}