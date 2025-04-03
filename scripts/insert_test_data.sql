USE HackatonDB;
GO

-- Inserir médicos de teste
INSERT INTO Medicos (Nome, CRM, Especialidade, Senha) VALUES
('Dr. João Silva', '123456SP', 'Clínico Geral', 'senha123'),
('Dra. Maria Santos', '789012SP', 'Cardiologista', 'senha123'),
('Dr. Pedro Oliveira', '345678SP', 'Pediatra', 'senha123'),
('Dra. Ana Costa', '901234SP', 'Dermatologista', 'senha123'),
('Dr. Carlos Souza', '567890SP', 'Ortopedista', 'senha123');

-- Inserir pacientes de teste
INSERT INTO Pacientes (Nome, CPF, Email, Telefone, Senha) VALUES
('José Pereira', '12345678901', 'jose@email.com', '11999887766', 'senha123'),
('Mariana Lima', '23456789012', 'mariana@email.com', '11988776655', 'senha123'),
('Roberto Santos', '34567890123', 'roberto@email.com', '11977665544', 'senha123'),
('Fernanda Silva', '45678901234', 'fernanda@email.com', '11966554433', 'senha123'),
('Paulo Oliveira', '56789012345', 'paulo@email.com', '11955443322', 'senha123');

-- Inserir agendas de teste (próximos 5 dias úteis)
DECLARE @DataInicial DATE = GETDATE();
DECLARE @DataFinal DATE = DATEADD(DAY, 14, GETDATE());
DECLARE @DataAtual DATE = @DataInicial;

WHILE @DataAtual <= @DataFinal
BEGIN
    -- Pular finais de semana
    IF DATEPART(WEEKDAY, @DataAtual) NOT IN (1, 7) -- 1 = Domingo, 7 = Sábado
    BEGIN
        -- Agenda para cada médico
        INSERT INTO Agendas (MedicoId, Data, HorarioInicio, HorarioFim)
        SELECT 
            Id AS MedicoId,
            @DataAtual AS Data,
            '08:00' AS HorarioInicio,
            '17:00' AS HorarioFim
        FROM Medicos;
    END
    
    SET @DataAtual = DATEADD(DAY, 1, @DataAtual);
END

-- Inserir algumas consultas de teste
INSERT INTO Consultas (MedicoId, PacienteId, AgendaId, DataHora, Status) 
SELECT TOP 5
    m.Id AS MedicoId,
    p.Id AS PacienteId,
    a.Id AS AgendaId,
    CAST(CAST(a.Data AS DATETIME) + CAST(DATEADD(HOUR, 9 + (ROW_NUMBER() OVER (ORDER BY m.Id)), '00:00') AS DATETIME) AS DATETIME) AS DataHora,
    'Pendente' AS Status
FROM Medicos m
CROSS JOIN Pacientes p
INNER JOIN Agendas a ON m.Id = a.MedicoId
WHERE a.Data >= GETDATE()
ORDER BY NEWID();

-- Atualizar algumas consultas com status diferentes
UPDATE TOP (1) Consultas SET Status = 'Aceita' WHERE Status = 'Pendente';
UPDATE TOP (1) Consultas SET Status = 'Recusada', Justificativa = 'Médico indisponível' WHERE Status = 'Pendente';
UPDATE TOP (1) Consultas SET Status = 'Realizada' WHERE Status = 'Pendente';
UPDATE TOP (1) Consultas SET Status = 'Cancelada', Justificativa = 'Paciente solicitou cancelamento' WHERE Status = 'Pendente'; 