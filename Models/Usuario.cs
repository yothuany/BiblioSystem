namespace BiblioSystem.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty; // hash BCrypt

    public int MembroIdMembro { get; set; }
    public Membro Membro { get; set; } = null!;
}
