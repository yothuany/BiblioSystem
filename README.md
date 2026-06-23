# 📚 BiblioSystem

**Documentação de Software (PDF) e Banco de Dados (SQL)  — BiblioSystem**

🔗 Acesse a pasta com os arquivos: https://drive.google.com/drive/folders/1KeXwlFNlDPXgaSrwW2U0FhOQrlWvQP3N?usp=sharing

---
## 📑 Sumário

- [🧪 Usuários de teste](#-usuários-de-teste)
- [🔐 Autenticação e autorização](#-autenticação-e-autorização)
- [📖 Sobre o projeto](#-sobre-o-projeto)
- [👥 Perfis de acesso (`Perfil`)](#-perfis-de-acesso-perfil)
- [🛡️ Como a senha é protegida no banco](#️-como-a-senha-é-protegida-no-banco)
- [🛠 Tecnologias utilizadas](#-tecnologias-utilizadas)
- [🗂 Modelo de domínio](#-modelo-de-domínio)
- [📁 Estrutura de pastas](#-estrutura-de-pastas)
- [🌐 Endpoints da API](#-endpoints-da-api)
- [📐 Regras de negócio](#-regras-de-negócio)
---

API REST para gerenciamento de uma biblioteca, desenvolvida em **C# / ASP.NET Core 8**.

## 🧪 Usuários de teste
(login rápido)

Para testar a API sem precisar cadastrar nada, use estas credenciais:
Como o cadastro de novos usuários (`POST /usuarios/Cadastrar`) exige um token de **Administrador**, o primeiro usuário precisa ser inserido manualmente. Abaixo estão os dois usuários de teste com seus hashes prontos.

| Perfil | E-mail | Senha |
|--------|--------|-------|
| **Administrador** (acesso total) | `admin@gmail.com` | `abacaxi` |
| **Bibliotecário** (acesso restrito) | `bibliotecario@gmail.com` | `banana` |

## 🔐 Autenticação e autorização
**Como usar:**

Antes de usar qualquer endpoint do sistema, é necessário **fazer login** e usar o token retornado nas próximas requisições.

### Como fazer login

1. Envie e-mail e senha para:
   ```
   POST /auth/Login
   ```
   ```json
   {
     "email": "admin@gmail.com",
     "senha": "abacaxi"
   }
   ```
2. Se as credenciais forem válidas, a API retorna um token JWT:
   ```json
   {
     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
   }
   ```
3. Use esse token em **todas as outras requisições**, no header:
   ```
   Authorization: Bearer {seu_token}
   ```
   O token tem validade de **3 horas**. Após esse período, é necessário logar novamente.
---

## 📖 Sobre o projeto

O **BiblioSystem** é o back-end (API) de um sistema de gerenciamento de biblioteca. Ele permite que uma instituição controle:

- O **acervo**: livros, autores e categorias (relações N:N entre eles).
- Os **exemplares físicos** de cada livro (com status "Disponível" / "Emprestado").
- Os **membros** da biblioteca (leitores que pegam livros emprestados).
- Os **empréstimos**, com data prevista de devolução e cálculo automático de multa em caso de atraso.
- As **reservas** de livros que estejam indisponíveis no momento.
- Os **usuários do sistema** (funcionários e administradores) que acessam a API, com login via e-mail/senha e emissão de token JWT.

O projeto segue uma arquitetura em camadas (**Controller → Service → DbContext**), com DTOs de entrada e saída, tratamento de erros centralizado e paginação genérica e reutilizável para todas as listagens.

---
### 👥 Perfis de acesso (`Perfil`)

Cada usuário cadastrado na tabela `Usuario` possui um campo **`Perfil`**, que é incluído no token como uma claim de *Role*. Na prática, o sistema reconhece dois níveis de acesso:

| Perfil | Quem é | O que pode fazer |
|---|---|---|
| **Administrador** | Gestor/responsável pelo sistema | Acesso total: além de usar todas as funcionalidades da biblioteca, é o **único perfil autorizado** a gerenciar as contas de outros usuários (listar, cadastrar, editar e remover funcionários/bibliotecários) em `/usuarios` |
| **Funcionário / Bibliotecário** (qualquer outro valor de `Perfil`, ex.: `"Bibliotecario"` ou `"Funcionario"`) | Equipe do dia a dia da biblioteca | Pode autenticar-se normalmente e operar os recursos da biblioteca (livros, autores, categorias, exemplares, membros, empréstimos e reservas), mas **não tem acesso** às rotas de `/usuarios` para gerenciar outras contas |

> ⚠️ O valor de `Perfil` é um texto livre definido no momento do cadastro do usuário — o sistema não restringe quais nomes podem ser usados. A única verificação de permissão feita no código é: **a rota `/usuarios` exige que o `Perfil` seja exatamente `"Administrador"`** (`[Authorize(Roles = "Administrador")]`). Para os demais módulos, basta estar autenticado (qualquer perfil) para ter acesso.



### 🛡️ Como a senha é protegida no banco

O projeto **nunca grava a senha em texto puro**. O hashing é feito pela classe `Microsoft.AspNetCore.Identity.PasswordHasher<Usuario>` (a mesma usada pelo ASP.NET Core Identity), tanto no cadastro (`UsuarioController.Create`) quanto na verificação do login (`AuthController.Login`):

```csharp
// Ao cadastrar
usuario.Senha = _passwordHasher.HashPassword(usuario, dto.Senha);

// Ao logar
var resultado = _hasher.VerifyHashedPassword(usuario, usuario.Senha, user.Senha);
```

Esse hasher usa o algoritmo **PBKDF2 com HMAC-SHA256**, com os seguintes parâmetros (versão 3, padrão do .NET):

| Parâmetro | Valor |
|---|---|
| Algoritmo base | PBKDF2 (HMAC-SHA256) |
| Iterações | 100.000 |
| Tamanho do salt | 128 bits (16 bytes), gerado aleatoriamente a cada hash |
| Tamanho da subkey (hash final) | 256 bits (32 bytes) |

O resultado salvo na coluna `senha` **não é só o hash** — é um único valor em Base64 que concatena: 1 byte de marcador de versão + identificador do algoritmo + número de iterações + tamanho do salt + o salt em si + o hash (subkey). Por isso não existe uma coluna separada de "salt" na tabela: o salt já vem embutido dentro do próprio valor da coluna `senha`.

Isso traz duas consequências importantes:

- **O processo é unidirecional**: não é possível "descriptografar" a senha a partir do hash — ao logar, o sistema recalcula o hash da senha digitada usando o mesmo salt e parâmetros guardados, e apenas compara os resultados.
- **Duas senhas iguais geram hashes diferentes**, porque o salt é aleatório a cada vez — é por isso que os dois hashes de exemplo acima (de `abacaxi` e `banana`) não seguem nenhum padrão visualmente parecido, mesmo sendo senhas curtas.

---
## 🛠 Tecnologias utilizadas

| Camada | Tecnologia |
|---|---|
| Linguagem / Framework | C# · .NET 8 (`net8.0`) · ASP.NET Core Web API |
| Banco de dados | MySQL 8 |
| ORM | Entity Framework Core 8 + [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) |
| Convenção de nomes do banco | `EFCore.NamingConventions` (snake_case) |
| Autenticação | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Hash de senha | `Microsoft.AspNetCore.Identity.PasswordHasher` |
| Mapeamento objeto-objeto | AutoMapper |
| Versionamento de API | `Asp.Versioning.Mvc` (versão padrão `v1`, via segmento de URL) |
| Documentação interativa | Swagger / Swashbuckle (com suporte a anotações e Bearer token) |

---

## 🗂 Modelo de domínio

As principais entidades do sistema e seus relacionamentos:

| Entidade | Descrição | Relacionamentos |
|---|---|---|
| **Livro** | Título, ISBN, ano de publicação, editora | N:N com `Autor` e `Categoria` · 1:N com `Exemplar` e `Reserva` |
| **Autor** | Nome e biografia | N:N com `Livro` |
| **Categoria** | Nome e descrição (gênero/assunto) | N:N com `Livro` |
| **Exemplar** | Código físico do exemplar e status (`Disponível` / `Emprestado`) | N:1 com `Livro` · 1:N com `Emprestimo` |
| **Membro** | Nome, e-mail, telefone, CPF (leitor da biblioteca) | 1:N com `Reserva` e `Emprestimo` |
| **Emprestimo** | Datas de empréstimo/devolução prevista/devolução real, status e valor da multa | N:1 com `Membro` e `Exemplar` |
| **Reserva** | Data da reserva e status (`Ativa` / `Cancelada`) | N:1 com `Membro` e `Livro` |
| **Usuario** | Conta de acesso à API (nome, e-mail, senha hash, perfil) | usada apenas para login/autenticação |

As tabelas de relacionamento N:N (`Livro_Autor` e `Livro_Categoria`) são geradas automaticamente pelo Entity Framework Core através de `UsingEntity`, configurado em `DataContexts/AppDbContext.cs`.

---

## 📁 Estrutura de pastas

```
BiblioSystem/
├── Controllers/              # Endpoints HTTP (camada de apresentação)
│   ├── Filters/               # Filtros de busca/paginação por recurso (LivroFilter, EmprestimoFilter, etc.)
│   ├── AuthController.cs
│   ├── LivroController.cs
│   ├── AutorController.cs
│   ├── CategoriaController.cs
│   ├── ExemplarController.cs
│   ├── MembroController.cs
│   ├── EmprestimoController.cs
│   ├── ReservaController.cs
│   └── UsuarioController.cs
├── Services/                 # Regras de negócio (camada de serviço)
├── DataContexts/
│   └── AppDbContext.cs        # Contexto do Entity Framework Core
├── Models/                   # Entidades mapeadas para o banco (MySQL)
├── Dtos/                     # Objetos de entrada (requisição)
│   └── Responses/             # Objetos de saída (resposta)
├── Profile/
│   └── BibliotecaProfile.cs   # Configuração do AutoMapper
├── Helpers/
│   └── Paginated/             # Paginação genérica reutilizável (IPaginatedFilter, Paginate<T>, PaginatedResponse)
├── Exceptions/                # Exceções de negócio convertidas em respostas HTTP
├── Properties/
│   └── launchSettings.json
├── Program.cs                 # Configuração da aplicação (DI, JWT, Swagger, EF Core, AutoMapper)
├── appsettings.json           # Connection string e configurações de JWT
└── BiblioSystem.csproj
```

---

## 🌐 Endpoints da API

Todas as rotas abaixo (exceto login) exigem o header `Authorization: Bearer {token}`, salvo indicação contrária.

### Autenticação
| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/auth/Login` | Realiza login e retorna o token JWT | Público |

### Usuários do sistema
| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `GET` | `/usuarios/Consultar` | Lista todos os usuários | Administrador |
| `POST` | `/usuarios/Cadastrar` | Cria um novo usuário | Administrador |
| `PUT` | `/usuarios/Editar{id}` | Atualiza um usuário | Administrador |
| `DELETE` | `/usuarios/Remover{id}` | Remove um usuário (não permite autoexclusão) | Administrador |

### Livros
| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/livro/Consultar` | Lista/busca livros (por ID, título, ISBN ou autor) com paginação |
| `POST` | `/livro/Cadastrar` | Cadastra um novo livro |
| `PUT` | `/livro/Editar/{id}` | Atualiza um livro |
| `DELETE` | `/livro/Remover/{id}` | Remove um livro |

### Autores
| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/autor/Consultar` | Lista/busca autores |
| `POST` | `/autor/Cadastrar` | Cadastra um novo autor |
| `PUT` | `/autor/Editar/{id}` | Atualiza um autor |
| `DELETE` | `/autor/Remover/{id}` | Remove um autor |

### Categorias
| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/categoria/Consultar` | Lista/busca categorias |
| `POST` | `/categoria/Cadastrar` | Cadastra uma nova categoria |
| `PUT` | `/categoria/Editar/{id}` | Atualiza uma categoria |
| `DELETE` | `/categoria/Remover/{id}` | Remove uma categoria |

### Exemplares
| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/exemplar/Consultar` | Lista/busca exemplares (por código, status ou ISBN do livro) |
| `POST` | `/exemplar/Cadastrar` | Cadastra um novo exemplar (status inicial: `Disponível`) |
| `PUT` | `/exemplar/Editar/{id}` | Atualiza um exemplar |
| `DELETE` | `/exemplar/Remover/{id}` | Remove um exemplar (bloqueado se estiver com status `Emprestado`) |

### Membros
| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/membro/Consultar` | Lista/busca membros |
| `POST` | `/membro/Cadastrar` | Cadastra um novo membro |
| `PUT` | `/membro/Editar/{id}` | Atualiza um membro |
| `DELETE` | `/membro/Remover/{id}` | Remove um membro |

### Empréstimos
| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/emprestimo/Consultar` | Lista/busca empréstimos (por ID, status, código do exemplar, nome ou CPF do membro) |
| `POST` | `/emprestimo/Cadastrar` | Registra um novo empréstimo (por CPF do membro e código do exemplar) |
| `PUT` | `/emprestimo/Devolver/{id}` | Registra a devolução e calcula a multa, se houver atraso |
| `DELETE` | `/emprestimo/Remove/{id}` | Remove um registro de empréstimo |

### Reservas
| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/reserva/Consultar` | Lista/busca reservas (por ID, status, livro ou membro) |
| `POST` | `/reserva/Cadastrar` | Cria uma nova reserva (por ISBN do livro e CPF do membro) |
| `PUT` | `/reserva/Cancelar/{id}` | Cancela uma reserva |
| `DELETE` | `/reserva/Remove/{id}` | Remove um registro de reserva |

#### Paginação e busca

Os endpoints `Consultar` aceitam parâmetros de query string padronizados, herdados de `PaginatedFilter`:

| Parâmetro | Tipo | Padrão | Descrição |
|---|---|---|---|
| `search` | string | – | Termo de busca (varia por recurso: nome, código, status, CPF etc.) |
| `page` | int | `1` | Página atual |
| `limit` | int | `25` | Itens por página (máx. 100) |

A resposta paginada (`PaginatedResponse<T>`) inclui `page`, `limit`, `totalItems`, `totalPages` e `data`.

---

## 📐 Regras de negócio

- **Multa por atraso**: ao registrar a devolução (`PUT /emprestimo/Devolver/{id}`), se a data de devolução for posterior à data prevista, a multa é calculada como **R$ 2,00 por dia de atraso**.
- **Status do exemplar**: um exemplar não pode ser removido enquanto estiver com status `Emprestado`.
- **Status do empréstimo**: um empréstimo já marcado como `Devolvido` não pode ser devolvido novamente.
- **Vínculo por CPF/ISBN/Código**: empréstimos e reservas são criados a partir do **CPF do membro** e do **código do exemplar** (empréstimo) ou **ISBN do livro** (reserva), e não diretamente pelo ID interno — a API resolve essas referências e retorna erro de negócio caso não existam.
- **Autoexclusão bloqueada**: um usuário administrador não pode excluir a própria conta.
- **E-mail único**: tanto para usuários quanto verificado no cadastro/edição, e-mails duplicados são rejeitados.
