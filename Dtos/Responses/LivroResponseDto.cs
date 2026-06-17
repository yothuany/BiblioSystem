namespace BiblioSystem.Dtos.Responses
{
    public class LivroResponseDto
    {
        public int Id { get; set; }

        public required string Titulo { get; set; }

        public required string ISBN { get; set; }

        public int AnoPublicacao { get; set; }

        public required string Editora { get; set; }

        public CategoriaResponseDto? Categoria { get; set; }

        public ICollection<AutorResponseDto>? Autores { get; set; }

    }
}