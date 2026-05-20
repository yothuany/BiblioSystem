using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos.Reserva;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services;

public class ReservaService(AppDbContext db, IMapper mapper)
{
    private IQueryable<Reserva> QueryComIncludes() =>
        db.Reservas
            .Include(r => r.Membro)
            .Include(r => r.Livro);

    public async Task<List<ReservaResponseDto>> GetAllAsync()
    {
        var reservas = await QueryComIncludes().ToListAsync();
        return mapper.Map<List<ReservaResponseDto>>(reservas);
    }

    public async Task<ReservaResponseDto> GetByIdAsync(int id)
    {
        var reserva = await QueryComIncludes().FirstOrDefaultAsync(r => r.IdReserva == id)
            ?? throw new NotFoundException($"Reserva com id {id} não encontrada.");
        return mapper.Map<ReservaResponseDto>(reserva);
    }

    // RF09 - Realizar reserva de livro indisponível
    public async Task<ReservaResponseDto> CreateAsync(ReservaCreateDto dto)
    {
        var membroExiste = await db.Membros.AnyAsync(m => m.IdMembro == dto.MembroIdMembro);
        if (!membroExiste)
            throw new NotFoundException($"Membro com id {dto.MembroIdMembro} não encontrado.");

        var livroExiste = await db.Livros.AnyAsync(l => l.IdLivro == dto.LivroIdLivro);
        if (!livroExiste)
            throw new NotFoundException($"Livro com id {dto.LivroIdLivro} não encontrado.");

        // Verifica se já existe reserva pendente deste membro para este livro
        var reservaExistente = await db.Reservas.AnyAsync(r =>
            r.MembroIdMembro == dto.MembroIdMembro &&
            r.LivroIdLivro == dto.LivroIdLivro &&
            r.Status == "pendente");

        if (reservaExistente)
            throw new BusinessException("Membro já possui uma reserva pendente para este livro.");

        var reserva = mapper.Map<Reserva>(dto);
        db.Reservas.Add(reserva);
        await db.SaveChangesAsync();

        return await GetByIdAsync(reserva.IdReserva);
    }

    public async Task CancelarAsync(int id)
    {
        var reserva = await db.Reservas.FindAsync(id)
            ?? throw new NotFoundException($"Reserva com id {id} não encontrada.");

        if (reserva.Status != "pendente")
            throw new BusinessException("Apenas reservas pendentes podem ser canceladas.");

        reserva.Status = "cancelada";
        await db.SaveChangesAsync();
    }
}
