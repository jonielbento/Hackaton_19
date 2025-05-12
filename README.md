# Sistema de Consultas Médicas

Sistema para gerenciamento de consultas médicas desenvolvido em .NET 8.

## Visão Geral

O backend do projeto Hackaton foi desenvolvido em .NET 8.0, utilizando uma arquitetura em camadas (N-Tier) com padrões de design modernos e boas práticas de desenvolvimento.

## Tecnologias Principais

### Framework e Linguagem
- .NET 8.0: Framework mais recente da Microsoft
- C#: Linguagem de programação principal
- ASP.NET Core: Framework web para construção de APIs RESTful

### Banco de Dados
- SQL Server: Banco de dados relacional
- Entity Framework Core: ORM (Object-Relational Mapping)
- Migrations: Sistema de controle de versão do banco de dados

### Segurança
- JWT (JSON Web Tokens): Autenticação e autorização
- BCrypt: Hash de senhas
- Bearer Authentication: Esquema de autenticação
- CORS: Configurado para permitir requisições cross-origin

### Documentação
- Swagger/OpenAPI: Documentação automática da API
- Health Checks: Monitoramento de saúde da aplicação

## Estrutura do Projeto

### Camadas
1. **API (Hackaton.Api)**
   - Controllers
   - Configurações
   - Middlewares
   - Segurança

2. **Application (Hackaton.Application)**
   - Serviços
   - DTOs
   - Interfaces de Serviço

3. **Domain (Hackaton.Domain)**
   - Entidades
   - Interfaces de Repositório
   - Regras de Negócio

4. **Infrastructure (Hackaton.Infrastructure)**
   - Repositórios
   - Contexto do Banco de Dados
   - Implementações de Persistência

### Controllers
- AdminController: Gerenciamento administrativo
- AuthController: Autenticação e autorização
- PacientesController: Gestão de pacientes
- MedicosController: Gestão de médicos
- ConsultasController: Gestão de consultas
- AgendasController: Gestão de agendas
- TestAuthController: Testes de autenticação

## Funcionalidades Principais

### Autenticação e Autorização
- Login com JWT
- Refresh Token
- Proteção de rotas
- Hash seguro de senhas

### Gestão de Usuários
- CRUD de Médicos
- CRUD de Pacientes
- Perfis de acesso

### Agendamento
- Gestão de Agendas
- Marcação de Consultas
- Disponibilidade de Horários

## Configurações e Middlewares

### Middlewares Implementados
- Tratamento global de exceções
- CORS
- Autenticação JWT
- Health Checks
- Swagger/OpenAPI

### Configurações de Segurança
- Validação de tokens JWT
- Configuração de CORS
- Proteção contra ataques comuns

## Dependências Principais
- Microsoft.AspNetCore.OpenApi (8.0.8)
- Swashbuckle.AspNetCore (6.5.0)
- Microsoft.EntityFrameworkCore.Design (8.0.8)
- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.4)
- System.IdentityModel.Tokens.Jwt (7.4.0)
- BCrypt.Net-Next (4.0.3)

## Endpoints da API

### Autenticação
- POST /api/auth/login
- POST /api/auth/refresh-token

### Médicos
- GET /api/medicos
- POST /api/medicos
- PUT /api/medicos/{id}
- DELETE /api/medicos/{id}

### Pacientes
- GET /api/pacientes
- POST /api/pacientes
- PUT /api/pacientes/{id}
- DELETE /api/pacientes/{id}

### Consultas
- GET /api/consultas
- POST /api/consultas
- PUT /api/consultas/{id}
- DELETE /api/consultas/{id}

### Agendas
- GET /api/agendas
- POST /api/agendas
- PUT /api/agendas/{id}
- DELETE /api/agendas/{id}

## Monitoramento e Saúde
- Endpoint de Health Check: /health
- Swagger UI: /swagger
- Logs de exceções
- Monitoramento de performance

## Boas Práticas Implementadas
- Injeção de Dependência
- Repository Pattern
- Service Layer Pattern
- Tratamento de Exceções
- Validação de Dados
- Separação de Responsabilidades

## Considerações de Segurança
- Autenticação JWT
- Hash de Senhas com BCrypt
- Proteção contra CSRF
- Validação de Tokens
- Headers de Segurança
- CORS Configurado
- Tratamento de Exceções Seguro

## Integração Contínua

O projeto utiliza GitHub Actions para automação de CI/CD 