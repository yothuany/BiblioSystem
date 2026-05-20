using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos.Autor;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services;

public class AutorService(AppDbContext db, IMapper mapper)
{
    public async Task<List<AutorResponseDto>> GetAllAsync()
    {
        var autores = await db.Autores.ToListAsync();
        return mapper.Map<List<AutorResponseDto>>(autores);
    }

    public async Task<AutorResponseDto> GetByIdAsync(int id)
    {
        var autor = await db.Autores.FindAsync(id)
            ?? throw new NotFoundException($"Autor com id {id} não encontrado.");
        return mapper.Map<AutorResponseDto>(autor);
    }

    public async Task<AutorResponseDto> CreateAsync(AutorCreateDto dto)
    {
        var autor = mapper.Map<Autor>(dto);
        db.Autores.Add(autor);
        await db.SaveChangesAsync();
        return mapper.Map<AutorResponseDto>(autor);
    }

    public async Task<AutorResponseDto> UpdateAsync(int id, AutorUpdateDto dto)
    {
        var autor = await db.Autores.FindAsync(id)
            ?? throw new NotFoundException($"Autor com id {id} não encontrado.");
        mapper.Map(dto, autor);
        await db.SaveChangesAsync();
        return mapper.Map<AutorResponseDto>(autor);
    }

    public async Task DeleteAsync(int id)
    {
        var autor = await db.Autores.FindAsync(id)
            ?? throw new NotFoundException($"Autor com id {id} não encontrado.");
        db.Autores.Remove(autor);
        await db.SaveChangesAsync();
    }
}
