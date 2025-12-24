# PhotoStudio

Sistema web para gerenciamento de estúdio fotográfico, desenvolvido com **ASP.NET Core Blazor (Razor Components)**, utilizando **Entity Framework Core** e **SQL Server** como banco de dados.

O projeto tem como objetivo centralizar o controle de clientes, ensaios fotográficos e demais funcionalidades administrativas de um estúdio de fotografia.

---

## Tecnologias Utilizadas

- .NET 8 / ASP.NET Core
- Blazor Web App (Interactive Server Render Mode)
- Entity Framework Core
- SQL Server
- Razor Components
- Dependency Injection
- HttpClient
- ToastService (notificações)

---

## Estrutura Básica do Projeto

```
PhotoStudio.app
│
├── Components/
├── Data/
├── Services/
├── wwwroot/
├── appsettings.json
├── Program.cs
└── PhotoStudio.app.csproj
```

---

## Configuração do Banco de Dados

A aplicação utiliza SQL Server via Entity Framework Core.

Exemplo de string de conexão (`appsettings.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PhotoStudioDb;User Id=sa;Password=SUASENHA;TrustServerCertificate=True;"
}
```
---

## Como Executar o Projeto

### Pré-requisitos

- .NET SDK 8 ou superior
- SQL Server
- Visual Studio 2022+ ou VS Code

### Passos

1. Clone o repositório
2. Ajuste a string de conexão
3. Execute:

```bash
dotnet run
```

4. Acesse:
```
https://localhost:5001
```

---

## Observações

- Utiliza Interactive Server Render Mode
- Comunicação em tempo real via SignalR
- Estrutura preparada para expansão (auth, APIs, mobile)

---

## Licença

Projeto destinado a fins educacionais e de desenvolvimento.
