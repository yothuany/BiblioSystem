using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class LivroDto
    {
        [Required(ErrorMessage = "O campo Titulo é obrigatório")]
        [MinLength(5, ErrorMessage = "Obrigatório mínimo de 5 caracteres")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "O campo Autor é obrigatório")]
        [MinLength(5, ErrorMessage = "Obrigatório mínimo de 5 caracteres")]
        public required string Autor { get; set; }

        [Required(ErrorMessage = "O campo Categoria é obrigatório")]
        public required int CategoriaId { get; set; }

        [Required(ErrorMessage = "O campo Ano é obrigatório")]
        [Range(1, 9999, ErrorMessage = "Ano inválido")]
        public required int Ano { get; set; }
    }
}