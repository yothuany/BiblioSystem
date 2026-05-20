using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos.Emprestimo;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services;

public class EmprestimoService(AppDbContext db, IMapper mapper)
{
    private const decimal MultaPorDia = 1.50m; // RF08 - valor da multa por dia de atraso

    private IQueryable<Emprestimo> QueryComIncludes() =>
        db.Emprestimos
            .Include(e => e.Membro)
            .Include(e => e.Exemplar);

    public async Task<List<EmprestimoResponseDto>> GetAllAsync()
    {
        var emprestimos = await QueryComIncludes().ToListAsync();
        return mapper.Map<List<EmprestimoResponseDto>>(emprestimos);
    }

    public async Task<EmprestimoResponseDto> GetByIdAsync(int id)
    {
        var emprestimo = await QueryComIncludes().FirstOrDefaultAsync(e => e.IdEmprestimo == id)
            ?? throw new NotFoundException($"Empréstimo com id {id} não encontrado.");
        return mapper.Map<EmprestimoResponseDto>(emprestimo);
    }

    public async Task<EmprestimoResponseDto> CreateAsync(EmprestimoCreateDto dto)
    {
        var membroExiste = await db.Membros.AnyAsync(m => m.IdMembro == dto.MembroIdMembro);
        if (!membroExiste)
            throw new NotFoundException($"Membro com id {dto.MembroIdMembro} não encontrado.");

        var exemplar = await db.Exemplares.FindAsync(dto.ExemplarIdExemplar)
            ?? throw new NotFoundException($"Exemplar com id {dto.ExemplarIdExemplar} não encontrado.");

        if (exemplar.Status != "disponivel")
            throw new BusinessException($"Exemplar {exemplar.Codigo} não está disponível para empréstimo. Status atual: {exemplar.Status}.");

        var emprestimo = mapper.Map<Emprestimo>(dto);
        exemplar.Status = "emprestado";

        db.Emprestimos.Add(emprestimo);
        await db.SaveChangesAsync();

        return await GetByIdAsync(emprestimo.IdEmprestimo);
    }

    public async Task<EmprestimoResponseDto> RegistrarDevolucaoAsync(int id, EmprestimoDevolucaoDto dto)
    {
        var emprestimo = await QueryComIncludes().FirstOrDefaultAsync(e => e.IdEmprestimo == id)
            ?? throw new NotFoundException($"Empréstimo com id {id} não encontrado.");

        if (emprestimo.DataDevolucao.HasValue)
            throw new BusinessException("Este empréstimo já foi devolvido.");

        emprestimo.DataDevolucao = dto.DataDevolucao;

        if (dto.DataDevolucao > emprestimo.DataPrevistaDevolucao)
        {
            var diasAtraso = dto.DataDevolucao.DayNumber - emprestimo.DataPrevistaDevolucao.DayNumber;
            emprestimo.ValorMulta = diasAtraso * MultaPorDia;
        }

        emprestimo.Exemplar.Status = "disponivel";

        await db.SaveChangesAsync();
        return mapper.Map<EmprestimoResponseDto>(emprestimo);
    }
}
