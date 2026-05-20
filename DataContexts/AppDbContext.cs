using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.DataContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Livro> Livros { get; set; }
    public DbSet<Autor> Autores { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<LivroAutor> LivroAutores { get; set; }
    public DbSet<LivroCategoria> LiveCategorias { get; set; }
    public DbSet<Exemplar> Exemplares { get; set; }
    public DbSet<Membro> Membros { get; set; }
    public DbSet<Emprestimo> Emprestimos { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LivroAutor>()
            .HasKey(la => new { la.LivroIdLivro, la.AutorIdAutor });

        modelBuilder.Entity<LivroCategoria>()
            .HasKey(lc => new { lc.LivroIdLivro, lc.CategoriaIdCategoria });

        modelBuilder.Entity<LivroAutor>()
            .HasOne(la => la.Livro)
            .WithMany(l => l.LivroAutores)
            .HasForeignKey(la => la.LivroIdLivro);

        modelBuilder.Entity<LivroAutor>()
            .HasOne(la => la.Autor)
            .WithMany(a => a.LivroAutores)
            .HasForeignKey(la => la.AutorIdAutor);

        modelBuilder.Entity<LivroCategoria>()
            .HasOne(lc => lc.Livro)
            .WithMany(l => l.LiveCategoria)
            .HasForeignKey(lc => lc.LivroIdLivro);

        modelBuilder.Entity<LivroCategoria>()
            .HasOne(lc => lc.Categoria)
            .WithMany(c => c.LiveCategoria)
            .HasForeignKey(lc => lc.CategoriaIdCategoria);

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Membro)
            .WithOne(m => m.Usuario)
            .HasForeignKey<Usuario>(u => u.MembroIdMembro);

        modelBuilder.Entity<Emprestimo>()
            .Property(e => e.ValorMulta)
            .HasPrecision(10, 2);
    }
}
