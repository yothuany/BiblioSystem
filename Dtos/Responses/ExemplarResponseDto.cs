namespace BiblioSystem.Dtos.Responses
{
    public class ExemplarResponseDto
    {
        public int Id { get; set; }

        public required string CodigoExemplar { get; set; }

        public required string Status { get; set; }
    }
}