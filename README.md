# Sistema de Consultas Médicas

[![.NET CI/CD](https://github.com/seu-usuario/seu-repositorio/actions/workflows/ci.yml/badge.svg)](https://github.com/seu-usuario/seu-repositorio/actions/workflows/ci.yml)

Sistema para gerenciamento de consultas médicas desenvolvido em .NET 8.

## Arquitetura

O sistema segue uma arquitetura em camadas:

- **Hackaton.Domain**: Entidades e interfaces do domínio
- **Hackaton.Infrastructure**: Implementações de persistência e acesso a dados
- **Hackaton.Application**: Lógica de negócios e serviços
- **Hackaton.Api**: API REST para comunicação com o frontend
- **Hackaton.Web**: Interface de usuário Blazor WebAssembly
- **Hackaton.Tests**: Testes de integração

## Integração Contínua

O projeto utiliza GitHub Actions para automação de CI/CD com os seguintes processos:

1. **Build**: Compila todo o projeto
2. **Test**: Executa testes de integração
3. **Coverage**: Gera relatórios de cobertura de código
4. **Analyze**: Analisa a qualidade do código com SonarCloud

## Desenvolvimento

### Pré-requisitos

- .NET 8 SDK
- SQL Server

### Rodando localmente

```bash
# Restaurar dependências
dotnet restore

# Compilar projeto
dotnet build

# Executar testes
dotnet test

# Iniciar o backend
dotnet run --project Hackaton.Api

# Iniciar o frontend
dotnet run --project Hackaton.Web
```

## Testes

O projeto inclui testes de integração para validar os componentes do backend:

- Testes de repositórios (acesso a dados)
- Testes de serviços (lógica de negócios)

Para executar os testes com relatório de cobertura:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```
