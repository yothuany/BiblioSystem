using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos.Categoria;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services;

public class CategoriaService(AppDbContext db, IMapper mapper)
{
    public async Task<List<CategoriaResponseDto>> GetAllAsync()
    {
        var categorias = await db.Categorias.ToListAsync();
        return mapper.Map<List<CategoriaResponseDto>>(categorias);
    }

    public async Task<CategoriaResponseDto> GetByIdAsync(int id)
    {
        var categoria = await db.Categorias.FindAsync(id)
            ?? throw new NotFoundException($"Categoria com id {id} não encontrada.");
        return mapper.Map<CategoriaResponseDto>(categoria);
    }

    public async Task<CategoriaResponseDto> CreateAsync(CategoriaCreateDto dto)
    {
        var categoria = mapper.Map<Categoria>(dto);
        db.Categorias.Add(categoria);
        await db.SaveChangesAsync();
        return mapper.Map<CategoriaResponseDto>(categoria);
    }

    public async Task<CategoriaResponseDto> UpdateAsync(int id, CategoriaUpdateDto dto)
    {
        var categoria = await db.Categorias.FindAsync(id)
            ?? throw new NotFoundException($"Categoria com id {id} não encontrada.");
        mapper.Map(dto, categoria);
        await db.SaveChangesAsync();
        return mapper.Map<CategoriaResponseDto>(categoria);
    }

    public async Task DeleteAsync(int id)
    {
        var categoria = await db.Categorias.FindAsync(id)
            ?? throw new NotFoundException($"Categoria com id {id} não encontrada.");
        db.Categorias.Remove(categoria);
        await db.SaveChangesAsync();
    }
}
