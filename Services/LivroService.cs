using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos.Livro;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services;

public class LivroService(AppDbContext db, IMapper mapper)
{
    private IQueryable<Livro> QueryComIncludes() =>
        db.Livros
            .Include(l => l.LivroAutores).ThenInclude(la => la.Autor)
            .Include(l => l.LiveCategoria).ThenInclude(lc => lc.Categoria);

    public async Task<List<LivroResponseDto>> GetAllAsync()
    {
        var livros = await QueryComIncludes().ToListAsync();
        return mapper.Map<List<LivroResponseDto>>(livros);
    }

    public async Task<LivroResponseDto> GetByIdAsync(int id)
    {
        var livro = await QueryComIncludes().FirstOrDefaultAsync(l => l.IdLivro == id)
            ?? throw new NotFoundException($"Livro com id {id} não encontrado.");
        return mapper.Map<LivroResponseDto>(livro);
    }

    // RF10 - Pesquisa avançada por título, autor, categoria ou editora
    public async Task<List<LivroResponseDto>> PesquisarAsync(string? titulo, string? autor, string? categoria, string? editora)
    {
        var query = QueryComIncludes();

        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(l => l.Titulo.Contains(titulo));

        if (!string.IsNullOrWhiteSpace(autor))
            query = query.Where(l => l.LivroAutores.Any(la => la.Autor.Nome.Contains(autor)));

        if (!string.IsNullOrWhiteSpace(categoria))
            query = query.Where(l => l.LiveCategoria.Any(lc => lc.Categoria.Nome.Contains(categoria)));

        if (!string.IsNullOrWhiteSpace(editora))
            query = query.Where(l => l.Editora.Contains(editora));

        var livros = await query.ToListAsync();
        return mapper.Map<List<LivroResponseDto>>(livros);
    }

    public async Task<LivroResponseDto> CreateAsync(LivroCreateDto dto)
    {
        var isbnExistente = await db.Livros.AnyAsync(l => l.Isbn == dto.Isbn);
        if (isbnExistente)
            throw new BusinessException("ISBN já cadastrado.");

        var livro = new Livro
        {
            Titulo = dto.Titulo,
            Isbn = dto.Isbn,
            AnoPublicacao = dto.AnoPublicacao,
            Editora = dto.Editora
        };

        db.Livros.Add(livro);
        await db.SaveChangesAsync();

        foreach (var autorId in dto.AutorIds)
            db.LivroAutores.Add(new LivroAutor { LivroIdLivro = livro.IdLivro, AutorIdAutor = autorId });

        foreach (var catId in dto.CategoriaIds)
            db.LiveCategorias.Add(new LivroCategoria { LivroIdLivro = livro.IdLivro, CategoriaIdCategoria = catId });

        await db.SaveChangesAsync();
        return await GetByIdAsync(livro.IdLivro);
    }

    public async Task<LivroResponseDto> UpdateAsync(int id, LivroUpdateDto dto)
    {
        var livro = await db.Livros.FindAsync(id)
            ?? throw new NotFoundException($"Livro com id {id} não encontrado.");
        mapper.Map(dto, livro);
        await db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var livro = await db.Livros.FindAsync(id)
            ?? throw new NotFoundException($"Livro com id {id} não encontrado.");
        db.Livros.Remove(livro);
        await db.SaveChangesAsync();
    }
}
