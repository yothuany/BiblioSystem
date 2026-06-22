using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class LivroDto
    {
        [Required(ErrorMessage = "O campo Título é obrigatório")]
        [MinLength(3, ErrorMessage = "Obrigatório mínimo de 3 caracteres")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "O campo ISBN é obrigatório")]
        public required string Isbn { get; set; }

        [Required(ErrorMessage = "O campo Ano de Publicação é obrigatório")]
        [Range(1, 2100, ErrorMessage = "O ano de publicação deve ser um ano válido")]
        public required int AnoPublicacao { get; set; }

        public string? Editora { get; set; }

        [Required(ErrorMessage = "Obrigatório o envio de pelo menos um autor")]
        public required List<int> AutoresIds { get; set; }

        [Required(ErrorMessage = "Obrigatório o envio de pelo menos uma categoria")]
        public required List<int> CategoriasIds { get; set; }
    }
}