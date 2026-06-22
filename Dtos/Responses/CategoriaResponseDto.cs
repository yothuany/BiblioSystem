namespace BiblioSystem.Dtos.Responses
{
    public class CategoriaResponseDto
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public string? Descricao { get; set; }
    }
}