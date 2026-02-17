# Alterdata Finance API

API REST para gerenciamento financeiro pessoal desenvolvida como teste técnico para vaga de Programador.

**[Swagger em Produção](https://alterdata-finance-api-h0ddd7c8gvcfgeh5.canadacentral-01.azurewebsites.net/swagger/index.html)** · **[Health Check](https://alterdata-finance-api-h0ddd7c8gvcfgeh5.canadacentral-01.azurewebsites.net/api/HealthCheck)** · **[Frontend](https://alterdata-finance-web.vercel.app)**

---

## Sobre o Projeto

Aplicativo de gerenciamento financeiro pessoal que permite cadastrar, editar e excluir receitas e despesas, gerar relatórios por período (com exportação em CSV e PDF), e visualizar um dashboard com gráficos mensais.

---

## Tecnologias

| Camada         | Tecnologias                                               |
| -------------- | --------------------------------------------------------- |
| Runtime        | .NET 8 / C# 12                                           |
| Banco de Dados | PostgreSQL 17 + Entity Framework Core 8                  |
| Autenticação   | JWT Bearer (Microsoft.AspNetCore.Authentication)          |
| Validação      | FluentValidation                                         |
| PDF            | QuestPDF (Community)                                     |
| Documentação   | Swagger / Swashbuckle                                    |
| CI/CD          | GitHub Actions → Azure App Service                       |
| Containers     | Docker Compose (PostgreSQL)                              |

---

## Arquitetura

O projeto segue **Clean Architecture** com fluxo de dependência de fora para dentro:

```
┌─────────────────────────────────────────────────────────┐
│  API Layer            Controllers, Middleware, Swagger   │
├─────────────────────────────────────────────────────────┤
│  Application Layer    Services, DTOs, Validators         │
├─────────────────────────────────────────────────────────┤
│  Infrastructure Layer DbContext, Repositories, Migrations│
├─────────────────────────────────────────────────────────┤
│  Domain Layer         Entities, Enums, Interfaces        │
└─────────────────────────────────────────────────────────┘
```

- **Domain** — camada mais interna, zero dependências externas. Define entidades, enums e contratos (interfaces de repositório).
- **Application** — lógica de negócio. Serviços, DTOs, validações e mapeamentos. Depende apenas do Domain.
- **Infrastructure** — implementação de acesso a dados. EF Core, repositórios concretos e migrations. Implementa interfaces do Domain.
- **API** — ponto de entrada. Controllers, middleware de exceções e configuração de DI/auth.

---

## Estrutura do Projeto

```
src/
├── AlterdataFinanceApi.Domain/            # Entidades, Enums, Interfaces
│   ├── Entities/
│   │   ├── BaseEntity.cs                  # Id (Guid), CreatedAt, UpdatedAt
│   │   ├── Transaction.cs                 # Receitas e despesas
│   │   └── Administrator.cs               # Usuários do sistema
│   ├── Enums/
│   │   └── TransactionType.cs             # Expense | Revenue
│   ├── Interfaces/
│   │   ├── ITransactionRepository.cs
│   │   ├── ITransactionQueryRepository.cs
│   │   └── IAdministratorRepository.cs
│   └── Models/
│       └── MonthlySummary.cs
│
├── AlterdataFinanceApi.Application/       # Lógica de negócio
│   ├── DTOs/                              # Request/Response por feature
│   ├── Interfaces/                        # Contratos de serviço (ISP)
│   ├── Services/                          # Implementações
│   ├── Validators/                        # FluentValidation
│   ├── Mappings/                          # Extension methods (sem AutoMapper)
│   └── DependencyInjection.cs
│
├── AlterdataFinanceApi.Infrastructure/    # Acesso a dados
│   ├── Data/
│   │   ├── AppDbContext.cs                # Auditoria automática (CreatedAt/UpdatedAt)
│   │   ├── DatabaseSeeder.cs              # Seed com admin + ~300 transações
│   │   └── Configurations/               # Fluent API (IEntityTypeConfiguration)
│   ├── Repositories/
│   └── DependencyInjection.cs
│
└── AlterdataFinanceApi.API/               # Ponto de entrada
    ├── Controllers/                       # 5 controllers (Auth, Transactions, etc.)
    ├── Middlewares/
    │   └── ExceptionHandlingMiddleware.cs  # Tratamento global de exceções
    ├── Helpers/
    │   └── ReportPdfGenerator.cs          # Geração de PDF com QuestPDF
    └── Program.cs
```

---

## Endpoints da API

A documentação completa dos endpoints está disponível no **[Swagger](https://alterdata-finance-api-h0ddd7c8gvcfgeh5.canadacentral-01.azurewebsites.net/swagger/index.html)**.

---

## Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (para o PostgreSQL)

### 1. Subir o banco de dados

```bash
docker compose up -d
```

### 2. Executar a API

```bash
dotnet run --project src/AlterdataFinanceApi.API
```

A API estará disponível em `http://localhost:5027`.
O Swagger estará em `http://localhost:5027/swagger`.

> As migrations e o seed (admin + transações de exemplo) rodam automaticamente na primeira execução.

### Comandos úteis

```bash
# Build
dotnet build

# Criar migration
dotnet ef migrations add <Nome> \
  -p src/AlterdataFinanceApi.Infrastructure \
  -s src/AlterdataFinanceApi.API

# Aplicar migrations
dotnet ef database update \
  -p src/AlterdataFinanceApi.Infrastructure \
  -s src/AlterdataFinanceApi.API
```

---

## Deploy

Deploy automatizado via **GitHub Actions** no **Azure App Service** (região Canada Central). A cada push na branch `main`, o workflow faz build, publish e deploy automático.

---

## Autor

**Paulo Ricardo**
