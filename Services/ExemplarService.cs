using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos.Exemplar;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services;

public class ExemplarService(AppDbContext db, IMapper mapper)
{
    public async Task<List<ExemplarResponseDto>> GetAllAsync()
    {
        var exemplares = await db.Exemplares.Include(e => e.Livro).ToListAsync();
        return mapper.Map<List<ExemplarResponseDto>>(exemplares);
    }

    public async Task<ExemplarResponseDto> GetByIdAsync(int id)
    {
        var exemplar = await db.Exemplares.Include(e => e.Livro).FirstOrDefaultAsync(e => e.IdExemplar == id)
            ?? throw new NotFoundException($"Exemplar com id {id} não encontrado.");
        return mapper.Map<ExemplarResponseDto>(exemplar);
    }

    public async Task<ExemplarResponseDto> CreateAsync(ExemplarCreateDto dto)
    {
        var livroExiste = await db.Livros.AnyAsync(l => l.IdLivro == dto.LivroIdLivro);
        if (!livroExiste)
            throw new NotFoundException($"Livro com id {dto.LivroIdLivro} não encontrado.");

        var exemplar = mapper.Map<Exemplar>(dto);
        db.Exemplares.Add(exemplar);
        await db.SaveChangesAsync();

        return await GetByIdAsync(exemplar.IdExemplar);
    }

    public async Task<ExemplarResponseDto> UpdateAsync(int id, ExemplarUpdateDto dto)
    {
        var exemplar = await db.Exemplares.Include(e => e.Livro).FirstOrDefaultAsync(e => e.IdExemplar == id)
            ?? throw new NotFoundException($"Exemplar com id {id} não encontrado.");
        mapper.Map(dto, exemplar);
        await db.SaveChangesAsync();
        return mapper.Map<ExemplarResponseDto>(exemplar);
    }

    public async Task DeleteAsync(int id)
    {
        var exemplar = await db.Exemplares.FindAsync(id)
            ?? throw new NotFoundException($"Exemplar com id {id} não encontrado.");
        db.Exemplares.Remove(exemplar);
        await db.SaveChangesAsync();
    }
}
