using Hackaton.Domain.Entities;
using Hackaton.Infrastructure.Data;
using System;
using System.Linq;

namespace Hackaton.Tests
{
    public static class SeedTestData
    {
        public static void InitializeDbForTests(HackatonDbContext db)
        {
            // Limpar o banco de dados
            if (db.Medicos.Any())
            {
                db.Medicos.RemoveRange(db.Medicos);
            }
            if (db.Pacientes.Any())
            {
                db.Pacientes.RemoveRange(db.Pacientes);
            }
            if (db.Agendas.Any())
            {
                db.Agendas.RemoveRange(db.Agendas);
            }
            if (db.Consultas.Any())
            {
                db.Consultas.RemoveRange(db.Consultas);
            }
            db.SaveChanges();

            // Adicionar médicos
            var medico1 = new Medico
            {
                Nome = "Dr. João Silva",
                CRM = "12345-SP",
                Especialidade = "Cardiologia",
                ValorConsulta = 200.00M,
                Senha = "senha123" // Em produção, seria um hash
            };

            var medico2 = new Medico
            {
                Nome = "Dra. Maria Santos",
                CRM = "67890-SP",
                Especialidade = "Neurologia",
                ValorConsulta = 250.00M,
                Senha = "senha456" // Em produção, seria um hash
            };

            db.Medicos.AddRange(medico1, medico2);
            db.SaveChanges();

            // Adicionar pacientes
            var paciente1 = new Paciente
            {
                Nome = "Pedro Oliveira",
                CPF = "123.456.789-00",
                Email = "pedro@example.com",
                Telefone = "(11) 98765-4321",
                Senha = "senha789" // Em produção, seria um hash
            };

            var paciente2 = new Paciente
            {
                Nome = "Ana Souza",
                CPF = "987.654.321-00",
                Email = "ana@example.com",
                Telefone = "(11) 91234-5678",
                Senha = "senha012" // Em produção, seria um hash
            };

            db.Pacientes.AddRange(paciente1, paciente2);
            db.SaveChanges();

            // Adicionar agendas
            var dataAtual = DateTime.Now.Date;

            var agenda1 = new Agenda
            {
                MedicoId = medico1.Id,
                DataHoraInicio = dataAtual.AddDays(1).AddHours(8),
                DataHoraFim = dataAtual.AddDays(1).AddHours(12),
                Disponivel = true
            };

            var agenda2 = new Agenda
            {
                MedicoId = medico2.Id,
                DataHoraInicio = dataAtual.AddDays(1).AddHours(14),
                DataHoraFim = dataAtual.AddDays(1).AddHours(18),
                Disponivel = true
            };

            db.Agendas.AddRange(agenda1, agenda2);
            db.SaveChanges();

            // Adicionar consultas
            var consulta1 = new Consulta
            {
                PacienteId = paciente1.Id,
                MedicoId = medico1.Id,
                AgendaId = agenda1.Id,
                DataHora = dataAtual.AddDays(1).AddHours(9),
                Status = StatusConsulta.Agendada,
                Valor = medico1.ValorConsulta
            };

            db.Consultas.Add(consulta1);
            db.SaveChanges();
        }
    }
} 