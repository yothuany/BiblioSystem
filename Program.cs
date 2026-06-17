using Asp.Versioning;

using BiblioSystem.DataContexts;
using BiblioSystem.Profiles;
using BiblioSystem.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;

using Microsoft.OpenApi.Models;

using System.Text;

using System.Text.Json.Serialization;

var builder =
    WebApplication
    .CreateBuilder(
        args
    );


// DATABASE

var connectionString =
    builder
    .Configuration
    .GetConnectionString(
        "mysql"
    );


builder
.Services
.AddDbContext<AppDbContext>(
options =>
options
.UseMySql(
connectionString,

new MySqlServerVersion(
new Version(
8,
0,
32
)
)
)
.UseSnakeCaseNamingConvention()
);


// CONTROLLERS

builder
.Services
.AddControllers()

.AddJsonOptions(
options =>
{
    options
    .JsonSerializerOptions
    .ReferenceHandler =
    ReferenceHandler
    .IgnoreCycles;

    options
    .JsonSerializerOptions
    .WriteIndented =
    true;
}
);


// VERSIONAMENTO

builder
.Services
.AddApiVersioning(
options =>
{
    options
    .DefaultApiVersion =
    new ApiVersion(
    1,
    0
    );

    options
    .AssumeDefaultVersionWhenUnspecified =
    true;

    options
    .ReportApiVersions =
    true;

    options
    .ApiVersionReader =
    new UrlSegmentApiVersionReader();
}
)

.AddApiExplorer(
options =>
{
    options
    .GroupNameFormat =
    "'v'VVV";

    options
    .SubstituteApiVersionInUrl =
    true;
}
);


// JWT

builder
.Services
.AddAuthentication(
JwtBearerDefaults
.AuthenticationScheme
)

.AddJwtBearer(
options =>
{
    options
    .TokenValidationParameters =
    new TokenValidationParameters
    {
        ValidateIssuer =
    true,

        ValidateAudience =
    true,

        ValidateLifetime =
    true,

        ValidateIssuerSigningKey =
    true,

        ValidIssuer =
    builder
    .Configuration[
    "Jwt:Issuer"
    ],

        ValidAudience =
    builder
    .Configuration[
    "Jwt:Audience"
    ],

        IssuerSigningKey =
    new SymmetricSecurityKey(

    Encoding
    .UTF8
    .GetBytes(

    builder
    .Configuration[
    "Jwt:Key"
    ]
    ??

    string.Empty

    )

    )

    };

}
);


// AUTORIZAÇÃO

builder
.Services
.AddAuthorization();


// SWAGGER

builder
.Services
.AddEndpointsApiExplorer();

builder
.Services
.AddSwaggerGen(
options =>
{

    options
    .SwaggerDoc(
    "v1",

    new OpenApiInfo
    {
        Title =
    "Biblioteca API",

        Version =
    "v1",

        Description =
    "API Sistema Biblioteca"
    }
    );


    options
    .AddSecurityDefinition(
    "Bearer",

    new OpenApiSecurityScheme
    {
        Name =
    "Authorization",

        Type =
    SecuritySchemeType
    .ApiKey,

        Scheme =
    "Bearer",

        BearerFormat =
    "JWT",

        In =
    ParameterLocation
    .Header,

        Description =
    "Bearer {token}"
    }
    );


    options
    .AddSecurityRequirement(

    new OpenApiSecurityRequirement
    {
{
new OpenApiSecurityScheme
{
Reference =
new OpenApiReference
{
Type =
ReferenceType
.SecurityScheme,

Id =
"Bearer"
}
},

Array
.Empty<string>()
}
    }

    );

}
);


// SERVICES

builder
.Services
.AddScoped<
LivroService
>();

builder
.Services
.AddScoped<
AutorService
>();

builder
.Services
.AddScoped<
CategoriaService
>();

builder
.Services
.AddScoped<
ExemplarService
>();

builder
.Services
.AddScoped<
MembroService
>();

builder
.Services
.AddScoped<
EmprestimoService
>();

builder
.Services
.AddScoped<
ReservaService
>();

builder
.Services
.AddScoped<
UsuarioService
>();

builder
.Services
.AddScoped<
AuthService
>();


// AUTOMAPPER

builder
.Services
.AddAutoMapper(
config =>
config
.AddProfile<
BibliotecaProfile
>()
);


// BUILD

var app =
builder
.Build();


// PIPELINE

if (
app
.Environment
.IsDevelopment()
)
{
    app
    .UseSwagger();

    app
    .UseSwaggerUI();
}


app
.UseAuthentication();

app
.UseAuthorization();

app
.MapControllers();


app
.Run();