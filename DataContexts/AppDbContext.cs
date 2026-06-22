using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.DataContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options
        ) : base(options) { }

        public DbSet<Livro> Livro { get; set; }
        public DbSet<Autor> Autor { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Membro> Membro { get; set; }
        public DbSet<Exemplar> Exemplar { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<Emprestimo> Emprestimo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
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
                    f => f.ToTable("Livro_Autor")
                );

            modelBuilder.Entity<Livro>()
                .HasMany(l => l.Categorias)
                .WithMany(c => c.Livro)
                .UsingEntity<Dictionary<string, object>>(
                    "LivroCategoria",
                    f => f
                        .HasOne<Categoria>()
                        .WithMany()
                        .HasForeignKey("Categoria_id_categoria"),
                    f => f
                        .HasOne<Livro>()
                        .WithMany()
                        .HasForeignKey("Livro_id_livro"),
                    f => f.ToTable("Livro_Categoria")
                );

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Membro)
                .WithMany()
                .HasForeignKey(e => e.MembroId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Exemplar)
                .WithMany()
                .HasForeignKey(e => e.ExemplarId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}