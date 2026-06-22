namespace BiblioSystem.Dtos.Responses
{
    public class ExemplarResponseDto
    {
        public int Id { get; set; }
        public required string Codigo { get; set; }
        public required string Status { get; set; }
        public int LivroId { get; set; }
        public string? LivroTitulo { get; set; }
    }
}