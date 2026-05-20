using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos.Membro;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services;

public class MembroService(AppDbContext db, IMapper mapper)
{
    public async Task<List<MembroResponseDto>> GetAllAsync()
    {
        var membros = await db.Membros.ToListAsync();
        return mapper.Map<List<MembroResponseDto>>(membros);
    }

    public async Task<MembroResponseDto> GetByIdAsync(int id)
    {
        var membro = await db.Membros.FindAsync(id)
            ?? throw new NotFoundException($"Membro com id {id} não encontrado.");
        return mapper.Map<MembroResponseDto>(membro);
    }

    public async Task<MembroResponseDto> CreateAsync(MembroCreateDto dto)
    {
        var cpfExistente = await db.Membros.AnyAsync(m => m.Cpf == dto.Cpf);
        if (cpfExistente)
            throw new BusinessException("CPF já cadastrado.");

        var membro = mapper.Map<Membro>(dto);
        db.Membros.Add(membro);
        await db.SaveChangesAsync();
        return mapper.Map<MembroResponseDto>(membro);
    }

    public async Task<MembroResponseDto> UpdateAsync(int id, MembroUpdateDto dto)
    {
        var membro = await db.Membros.FindAsync(id)
            ?? throw new NotFoundException($"Membro com id {id} não encontrado.");
        mapper.Map(dto, membro);
        await db.SaveChangesAsync();
        return mapper.Map<MembroResponseDto>(membro);
    }

    public async Task DeleteAsync(int id)
    {
        var membro = await db.Membros.FindAsync(id)
            ?? throw new NotFoundException($"Membro com id {id} não encontrado.");
        db.Membros.Remove(membro);
        await db.SaveChangesAsync();
    }
}
