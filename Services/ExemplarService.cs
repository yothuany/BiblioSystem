using AutoMapper;
using BiblioSystem.Controllers.Filters;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Dtos.Responses;
using BiblioSystem.Exceptions;
using BiblioSystem.Helpers.Paginated;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioSystem.Services
{
    public class ExemplarService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ExemplarService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<ExemplarResponseDto>> FindAll(ExemplarFilter filter)
        {
            try
            {
                var query = _context.Exemplar.Include(x => x.Livro).AsQueryable();

                if (filter.Search is not null)
                {
                    if (int.TryParse(filter.Search, out int idBuscado))
                    {
                        query = query.Where(x => x.Id == idBuscado);
                    }
                    else
                    {
                        query = query.Where(x => x.Codigo.Contains(filter.Search) ||
                                                 x.Status.Contains(filter.Search) ||
                                                 x.Livro!.Isbn.Contains(filter.Search));
                    }
                }
                return await Paginate<Exemplar>.Set<ExemplarResponseDto>(query, filter, _mapper);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Exemplar> Create(ExemplarDto data)
        {
            try
            {
                var livro = await _context.Livro.FirstOrDefaultAsync(l => l.Isbn == data.IsbnLivro);
                if (livro is null)
                {
                    throw new ErrorServiceException($"Livro com ISBN {data.IsbnLivro} não encontrado",
                        c => c.BadRequest(new { message = $"Não é possível cadastrar o exemplar: Livro com ISBN {data.IsbnLivro} não existe." }));
                }

                var exemplar = new Exemplar
                {
                    Codigo = data.Codigo,
                    LivroId = livro.Id,
                    Status = "Disponível"
                };

                await _context.Exemplar.AddAsync(exemplar);
                await _context.SaveChangesAsync();

                return exemplar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Exemplar> FindById(int id)
        {
            try
            {
                var exemplar = await _context.Exemplar.Include(x => x.Livro).FirstOrDefaultAsync(x => x.Id == id);

                if (exemplar is null)
                {
                    throw new ErrorServiceException($"Exemplar {id} não encontrado",
                        c => c.NotFound(new { message = $"Exemplar #{id} não encontrado" }));
                }

                return exemplar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Exemplar> Update(int id, ExemplarDto data)
        {
            try
            {
                var exemplar = await FindById(id);

                var livro = await _context.Livro.FirstOrDefaultAsync(l => l.Isbn == data.IsbnLivro);
                if (livro is null)
                {
                    throw new ErrorServiceException($"Livro com ISBN {data.IsbnLivro} não encontrado",
                        c => c.BadRequest(new { message = $"Não é possível atualizar o exemplar: Livro com ISBN {data.IsbnLivro} não existe." }));
                }

                exemplar.Codigo = data.Codigo;
                exemplar.LivroId = livro.Id;

                _context.Exemplar.Update(exemplar);
                await _context.SaveChangesAsync();

                return exemplar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Remove(int id)
        {
            try
            {
                var exemplar = await FindById(id);

                if (exemplar.Status == "Emprestado")
                {
                    throw new ErrorServiceException($"Não é possível remover o exemplar porque ele está emprestado.",
                        c => c.BadRequest(new { message = $"O exemplar código '{exemplar.Codigo}' não pode ser removido pois está com o status 'Emprestado'." }));
                }

                _context.Exemplar.Remove(exemplar);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}