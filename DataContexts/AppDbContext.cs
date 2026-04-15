using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BiblioSystem.DataContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Livro> Livros { get; set; }

    }
}