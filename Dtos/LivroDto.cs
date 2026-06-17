using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class LivroDto
    {
        [Required]
        public required string Titulo { get; set; }

        [Required]
        public required string ISBN { get; set; }

        [Required]
        public int AnoPublicacao { get; set; }

        [Required]
        public required string Editora { get; set; }

        [Required]
        public int CategoriaId { get; set; }
    }

}