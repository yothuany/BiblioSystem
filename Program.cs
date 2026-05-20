using BiblioSystem.DataContexts;
using BiblioSystem.Profiles;
using BiblioSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Banco de dados MySQL
var connectionString = builder.Configuration.GetConnectionString("mysql");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)))
);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(config => config.AddProfile<LivroProfile>());

// Services (injeção de dependência)
builder.Services.AddScoped<AutorService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<LivroService>();
builder.Services.AddScoped<MembroService>();
builder.Services.AddScoped<ExemplarService>();
builder.Services.AddScoped<EmprestimoService>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<UsuarioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
