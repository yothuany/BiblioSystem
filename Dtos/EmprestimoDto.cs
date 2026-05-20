namespace BiblioSystem.Dtos.Emprestimo;

public record EmprestimoCreateDto(
    int MembroIdMembro,
    int ExemplarIdExemplar,
    DateOnly DataPrevistaDevolucao
);

public record EmprestimoDevolucaoDto(DateOnly DataDevolucao);

public record EmprestimoResponseDto(
    int IdEmprestimo,
    DateOnly DataEmprestimo,
    DateOnly DataPrevistaDevolucao,
    DateOnly? DataDevolucao,
    decimal ValorMulta,
    int MembroIdMembro,
    string NomeMembro,
    int ExemplarIdExemplar,
    string CodigoExemplar
);
