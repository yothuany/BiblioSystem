namespace BiblioSystem.Dtos.Responses
{
    public class LivroResponseDto
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Isbn { get; set; }
        public required int AnoPublicacao { get; set; }
        public string? Editora { get; set; }

        public ICollection<AutorResponseDto> Autores { get; set; } = new List<AutorResponseDto>();
        public ICollection<CategoriaResponseDto> Categorias { get; set; } = new List<CategoriaResponseDto>();
    }
}