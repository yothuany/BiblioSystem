using ApiFinanceiro.Exceptions;
using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BiblioSystem.Services
{
    public class LivroService
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public LivroService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<Livro>> FindAll()
        {
            try
            {
                return await _context.Livros
                    .Include(d => d.Categoria)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Livro> Create(LivroDto data)
        {
            try
            {
                var categoriaExiste = await _context.Categorias.AnyAsync(x => x.Id == data.CategoriaId);

                if (!categoriaExiste)
                {
                    throw new ErrorServiceException($"Categoria não encontrada",
                        c => c.NotFound(new { message = $"Categoria #{data.CategoriaId} não encontrada" }));
                }

                var livro = _mapper.Map<Livro>(data);

                await _context.Livros.AddAsync(livro);
                await _context.SaveChangesAsync();

                return livro;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Livro> FindById(int id)
        {
            try
            {
                var livro = await _context.Livros.FirstOrDefaultAsync(x => x.Id == id);

                if (livro is null)
                {
                    throw new ErrorServiceException($"Livro ${id} não encontrado",
                        c => c.NotFound(new { message = $"Livro #{id} não encontrado" }));
                }

                return livro;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Livro> Update(int id, LivroUpdateDto data)
        {
            try
            {
                var livro = await FindById(id);

                var categoriaExiste = await _context.Categorias.AnyAsync(x => x.Id == data.CategoriaId);

                if (!categoriaExiste)
                {
                    throw new ErrorServiceException($"Categoria não encontrada",
                        c => c.NotFound(new { message = $"Categoria #{data.CategoriaId} não encontrada" }));
                }

                _mapper.Map(data, livro);

                _context.Livros.Update(livro);
                await _context.SaveChangesAsync();

                return livro;

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
                var livro = await FindById(id);

                _context.Livros.Remove(livro);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}