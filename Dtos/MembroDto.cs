namespace BiblioSystem.Dtos.Membro;

public record MembroCreateDto(
    string Nome,
    string Cpf,
    string Email,
    string Telefone
);

public record MembroUpdateDto(
    string Nome,
    string Email,
    string Telefone
);

public record MembroResponseDto(
    int IdMembro,
    string Nome,
    string Cpf,
    string Email,
    string Telefone,
    DateOnly DataCadastro
);
