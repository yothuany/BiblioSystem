namespace BiblioSystem.Dtos.Usuario;

public record UsuarioCreateDto(string Email, string Senha, int MembroIdMembro);
public record LoginDto(string Email, string Senha);
public record LoginResponseDto(string Token, string Nome, string Email);
