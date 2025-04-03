USE HackatonDB;
GO

-- Criar tabela de Médicos
CREATE TABLE Medicos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    CRM NVARCHAR(20) NOT NULL UNIQUE,
    Especialidade NVARCHAR(50) NOT NULL,
    Senha NVARCHAR(100) NOT NULL,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE()
);

-- Criar tabela de Pacientes
CREATE TABLE Pacientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    CPF NVARCHAR(11) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL,
    Telefone NVARCHAR(20) NOT NULL,
    Senha NVARCHAR(100) NOT NULL,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE()
);

-- Criar tabela de Agendas
CREATE TABLE Agendas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MedicoId INT NOT NULL,
    Data DATE NOT NULL,
    HorarioInicio TIME NOT NULL,
    HorarioFim TIME NOT NULL,
    Disponivel BIT NOT NULL DEFAULT 1,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MedicoId) REFERENCES Medicos(Id)
);

-- Criar tabela de Consultas
CREATE TABLE Consultas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MedicoId INT NOT NULL,
    PacienteId INT NOT NULL,
    AgendaId INT NOT NULL,
    DataHora DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pendente',
    Justificativa NVARCHAR(500) NULL,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MedicoId) REFERENCES Medicos(Id),
    FOREIGN KEY (PacienteId) REFERENCES Pacientes(Id),
    FOREIGN KEY (AgendaId) REFERENCES Agendas(Id)
); 