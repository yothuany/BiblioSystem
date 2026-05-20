namespace BiblioSystem.Dtos.Reserva;

public record ReservaCreateDto(int MembroIdMembro, int LivroIdLivro);

public record ReservaResponseDto(
    int IdReserva,
    DateOnly DataReserva,
    string Status,
    int MembroIdMembro,
    string NomeMembro,
    int LivroIdLivro,
    string TituloLivro
);
