using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class AutorDto
    {
        [Required(
            ErrorMessage = "O campo Nome é obrigatório"
        )]
        [MinLength(
            3,
            ErrorMessage = "Obrigatório mínimo de 3 caracteres"
        )]
        public required string Nome { get; set; }

        [Required(
            ErrorMessage = "O campo Biografia é obrigatória"
        )]
        [MinLength(
            10,
            ErrorMessage = "Obrigatório mínimo de 10 caracteres"
        )]
        public required string Biografia { get; set; }
    }
}