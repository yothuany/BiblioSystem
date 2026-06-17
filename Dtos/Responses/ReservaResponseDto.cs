namespace BiblioSystem.Dtos.Responses
{
    public class ReservaResponseDto
    {
        public int Id { get; set; }

        public DateTime DataReserva { get; set; }

        public required string Status { get; set; }

        public LivroResponseDto? Livro { get; set; }

        public MembroResponseDto? Membro { get; set; }
    }
}