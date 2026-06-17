using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.DataContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options
        ) : base(options) { }

        public DbSet<Livro> Livros { get; set; }

        public DbSet<Exemplar> Exemplares { get; set; }

        public DbSet<Membro> Membros { get; set; }

        public DbSet<Autor> Autores { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Emprestimo> Emprestimos { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder
        )
        {
            modelBuilder.Entity<Livro>()
                .HasMany(l => l.Autores)
                .WithMany(a => a.Livros)
                .UsingEntity<Dictionary<string, object>>(
                    "LivroAutor",

                    f => f
                        .HasOne<Autor>()
                        .WithMany()
                        .HasForeignKey("autor_id"),

                    f => f
                        .HasOne<Livro>()
                        .WithMany()
                        .HasForeignKey("livro_id"),

                    f => f.ToTable("livros_autores")
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}