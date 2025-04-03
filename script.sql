IF EXISTS (SELECT * FROM sys.databases WHERE name = 'HackatonDB')
BEGIN
    ALTER DATABASE HackatonDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE HackatonDB;
END
CREATE DATABASE HackatonDB;
GO
USE HackatonDB;
GO

CREATE TABLE Medicos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    CRM NVARCHAR(20) NOT NULL UNIQUE,
    Senha NVARCHAR(MAX) NOT NULL,
    SaltSenha NVARCHAR(MAX) NOT NULL,
    Especialidade NVARCHAR(50) NOT NULL,
    ValorConsulta DECIMAL(10,2) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Telefone NVARCHAR(20) NOT NULL
);

CREATE TABLE Pacientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    CPF NVARCHAR(11) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL,
    Senha NVARCHAR(MAX) NOT NULL,
    SaltSenha NVARCHAR(MAX) NOT NULL,
    Telefone NVARCHAR(20) NOT NULL,
    DataNascimento DATE NOT NULL
);

CREATE TABLE Agendas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MedicoId INT NOT NULL,
    DataHoraInicio DATETIME NOT NULL,
    DataHoraFim DATETIME NOT NULL,
    Disponivel BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (MedicoId) REFERENCES Medicos(Id)
);

CREATE TABLE Consultas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MedicoId INT NOT NULL,
    PacienteId INT NOT NULL,
    AgendaId INT NOT NULL,
    DataHora DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    Valor DECIMAL(10,2) NOT NULL,
    Justificativa NVARCHAR(MAX),
    FOREIGN KEY (MedicoId) REFERENCES Medicos(Id),
    FOREIGN KEY (PacienteId) REFERENCES Pacientes(Id),
    FOREIGN KEY (AgendaId) REFERENCES Agendas(Id)
);

INSERT INTO Medicos (Nome, CRM, Senha, SaltSenha, Especialidade, ValorConsulta, Email, Telefone)
VALUES ('Dr. João Silva', '123456SP', '123456', '', 'Clínico Geral', 200.00, 'joao.silva@email.com', '11999999999');

INSERT INTO Pacientes (Nome, CPF, Email, Senha, SaltSenha, Telefone, DataNascimento)
VALUES ('Maria Santos', '12345678900', 'maria.santos@email.com', '123456', '', '11988888888', '1990-01-01'); 