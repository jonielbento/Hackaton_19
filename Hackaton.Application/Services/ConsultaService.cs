using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Hackaton.Domain.Entities;
using Hackaton.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Application.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly HackatonDbContext _context;

        public ConsultaService(HackatonDbContext context)
        {
            _context = context;
        }

        public async Task<ConsultaDTO> AceitarConsultaAsync(int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
                return null;

            consulta.Status = StatusConsulta.Confirmada;
            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();

            return new ConsultaDTO
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                MedicoId = consulta.MedicoId,
                AgendaId = consulta.AgendaId,
                DataHora = consulta.DataHora,
                Status = consulta.Status,
                Valor = consulta.Valor,
                Justificativa = consulta.Justificativa,
                NomePaciente = consulta.Paciente.Nome,
                NomeMedico = consulta.Medico.Nome,
                Medico = new MedicoDTO
                {
                    Id = consulta.Medico.Id,
                    Nome = consulta.Medico.Nome,
                    CRM = consulta.Medico.CRM,
                    Especialidade = consulta.Medico.Especialidade,
                    ValorConsulta = consulta.Medico.ValorConsulta,
                    Telefone = consulta.Medico.Telefone,
                    Email = consulta.Medico.Email
                },
                Paciente = new PacienteDTO
                {
                    Id = consulta.Paciente.Id,
                    Nome = consulta.Paciente.Nome,
                    CPF = consulta.Paciente.CPF,
                    Email = consulta.Paciente.Email,
                    Telefone = consulta.Paciente.Telefone,
                    DataNascimento = consulta.Paciente.DataNascimento
                }
            };
        }

        public async Task<ConsultaDTO> CancelarConsultaAsync(int id, string justificativa)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Include(c => c.Agenda)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
                return null;

            consulta.Status = StatusConsulta.Cancelada;
            consulta.Justificativa = justificativa;

            var agenda = await _context.Agendas.FindAsync(consulta.AgendaId);
            if (agenda != null)
            {
                agenda.Disponivel = true;
                _context.Agendas.Update(agenda);
            }

            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();

            return new ConsultaDTO
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                MedicoId = consulta.MedicoId,
                AgendaId = consulta.AgendaId,
                DataHora = consulta.DataHora,
                Status = consulta.Status,
                Valor = consulta.Valor,
                Justificativa = consulta.Justificativa,
                NomePaciente = consulta.Paciente.Nome,
                NomeMedico = consulta.Medico.Nome,
                Medico = new MedicoDTO
                {
                    Id = consulta.Medico.Id,
                    Nome = consulta.Medico.Nome,
                    CRM = consulta.Medico.CRM,
                    Especialidade = consulta.Medico.Especialidade,
                    ValorConsulta = consulta.Medico.ValorConsulta,
                    Telefone = consulta.Medico.Telefone,
                    Email = consulta.Medico.Email
                },
                Paciente = new PacienteDTO
                {
                    Id = consulta.Paciente.Id,
                    Nome = consulta.Paciente.Nome,
                    CPF = consulta.Paciente.CPF,
                    Email = consulta.Paciente.Email,
                    Telefone = consulta.Paciente.Telefone,
                    DataNascimento = consulta.Paciente.DataNascimento
                }
            };
        }

        public async Task<ConsultaDTO> CreateAsync(ConsultaRegistroDTO consultaDTO)
        {
            Console.WriteLine($"Iniciando criação de consulta para paciente {consultaDTO.PacienteId} com médico {consultaDTO.MedicoId} na agenda {consultaDTO.AgendaId}");

            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var paciente = await _context.Pacientes.FindAsync(consultaDTO.PacienteId);
                if (paciente == null)
                {
                    Console.WriteLine($"Paciente {consultaDTO.PacienteId} não encontrado");
                    throw new Exception($"Paciente {consultaDTO.PacienteId} não encontrado");
                }

                var medico = await _context.Medicos.FindAsync(consultaDTO.MedicoId);
                if (medico == null)
                {
                    Console.WriteLine($"Médico {consultaDTO.MedicoId} não encontrado");
                    throw new Exception($"Médico {consultaDTO.MedicoId} não encontrado");
                }

                // Carrega a agenda com bloqueio exclusivo
                var agenda = await _context.Agendas
                    .FromSqlRaw("SELECT * FROM Agendas WITH (UPDLOCK, ROWLOCK) WHERE Id = {0}", consultaDTO.AgendaId)
                    .Include(a => a.Medico)
                    .FirstOrDefaultAsync();

                if (agenda == null)
                {
                    Console.WriteLine($"Agenda {consultaDTO.AgendaId} não encontrada");
                    throw new Exception($"Agenda {consultaDTO.AgendaId} não encontrada");
                }

                // Modificamos esta verificação para considerar como disponível uma agenda mesmo que não esteja marcada como disponível no campo Disponivel
                // A disponibilidade real será verificada pela existência de consultas ativas no horário específico
                
                // Verifica se já existe uma consulta ativa para esta agenda no horário específico selecionado pelo paciente
                var consultaExistente = await _context.Consultas
                    .AnyAsync(c => c.AgendaId == consultaDTO.AgendaId && 
                             c.DataHora.Hour == consultaDTO.DataHora.Hour &&
                             c.DataHora.Minute == consultaDTO.DataHora.Minute &&
                             c.Status != StatusConsulta.Cancelada && 
                             c.Status != StatusConsulta.Recusada);

                if (consultaExistente)
                {
                    Console.WriteLine($"Já existe uma consulta ativa para a agenda {consultaDTO.AgendaId} no horário {consultaDTO.DataHora:HH:mm}");
                    throw new Exception("Este horário já está reservado para outra consulta");
                }

                // Verifica se a data/hora da consulta já passou
                if (consultaDTO.DataHora < DateTime.Now)
                {
                    Console.WriteLine($"A data/hora {consultaDTO.DataHora} já passou");
                    throw new Exception("Não é possível agendar consultas para datas/horários passados");
                }

                var consulta = new Consulta
                {
                    PacienteId = consultaDTO.PacienteId,
                    MedicoId = consultaDTO.MedicoId,
                    AgendaId = consultaDTO.AgendaId,
                    DataHora = consultaDTO.DataHora,
                    Status = StatusConsulta.Agendada,
                    Valor = medico.ValorConsulta
                };

                Console.WriteLine($"Criando consulta: PacienteId={consulta.PacienteId}, MedicoId={consulta.MedicoId}, AgendaId={consulta.AgendaId}, DataHora={consulta.DataHora}");

                _context.Consultas.Add(consulta);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                Console.WriteLine($"Consulta {consulta.Id} criada com sucesso");

                return new ConsultaDTO
                {
                    Id = consulta.Id,
                    PacienteId = consulta.PacienteId,
                    MedicoId = consulta.MedicoId,
                    AgendaId = consulta.AgendaId,
                    DataHora = consulta.DataHora,
                    Status = consulta.Status,
                    Valor = consulta.Valor,
                    Justificativa = consulta.Justificativa,
                    NomePaciente = paciente.Nome,
                    NomeMedico = medico.Nome,
                    Medico = new MedicoDTO
                    {
                        Id = medico.Id,
                        Nome = medico.Nome,
                        CRM = medico.CRM,
                        Especialidade = medico.Especialidade,
                        ValorConsulta = medico.ValorConsulta,
                        Telefone = medico.Telefone,
                        Email = medico.Email
                    },
                    Paciente = new PacienteDTO
                    {
                        Id = paciente.Id,
                        Nome = paciente.Nome,
                        CPF = paciente.CPF,
                        Email = paciente.Email,
                        Telefone = paciente.Telefone,
                        DataNascimento = paciente.DataNascimento
                    }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Erro ao criar consulta: {ex.Message}");
                throw new Exception($"Erro ao criar consulta: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
                return false;

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ConsultaDTO>> GetAllAsync()
        {
            var consultas = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();

            return consultas.Select(c => new ConsultaDTO
            {
                Id = c.Id,
                PacienteId = c.PacienteId,
                MedicoId = c.MedicoId,
                AgendaId = c.AgendaId,
                DataHora = c.DataHora,
                Status = c.Status,
                Valor = c.Valor,
                Justificativa = c.Justificativa,
                NomePaciente = c.Paciente.Nome,
                NomeMedico = c.Medico.Nome,
                Medico = new MedicoDTO
                {
                    Id = c.Medico.Id,
                    Nome = c.Medico.Nome,
                    CRM = c.Medico.CRM,
                    Especialidade = c.Medico.Especialidade,
                    ValorConsulta = c.Medico.ValorConsulta,
                    Telefone = c.Medico.Telefone,
                    Email = c.Medico.Email
                },
                Paciente = new PacienteDTO
                {
                    Id = c.Paciente.Id,
                    Nome = c.Paciente.Nome,
                    CPF = c.Paciente.CPF,
                    Email = c.Paciente.Email,
                    Telefone = c.Paciente.Telefone,
                    DataNascimento = c.Paciente.DataNascimento
                }
            });
        }

        public async Task<ConsultaDTO> GetByIdAsync(int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
                return null;

            return new ConsultaDTO
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                MedicoId = consulta.MedicoId,
                AgendaId = consulta.AgendaId,
                DataHora = consulta.DataHora,
                Status = consulta.Status,
                Valor = consulta.Valor,
                Justificativa = consulta.Justificativa,
                NomePaciente = consulta.Paciente.Nome,
                NomeMedico = consulta.Medico.Nome,
                Medico = new MedicoDTO
                {
                    Id = consulta.Medico.Id,
                    Nome = consulta.Medico.Nome,
                    CRM = consulta.Medico.CRM,
                    Especialidade = consulta.Medico.Especialidade,
                    ValorConsulta = consulta.Medico.ValorConsulta,
                    Telefone = consulta.Medico.Telefone,
                    Email = consulta.Medico.Email
                },
                Paciente = new PacienteDTO
                {
                    Id = consulta.Paciente.Id,
                    Nome = consulta.Paciente.Nome,
                    CPF = consulta.Paciente.CPF,
                    Email = consulta.Paciente.Email,
                    Telefone = consulta.Paciente.Telefone,
                    DataNascimento = consulta.Paciente.DataNascimento
                }
            };
        }

        public async Task<IEnumerable<ConsultaDTO>> GetByMedicoIdAsync(int medicoId)
        {
            var consultas = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Where(c => c.MedicoId == medicoId)
                .ToListAsync();

            return consultas.Select(c => new ConsultaDTO
            {
                Id = c.Id,
                PacienteId = c.PacienteId,
                MedicoId = c.MedicoId,
                AgendaId = c.AgendaId,
                DataHora = c.DataHora,
                Status = c.Status,
                Valor = c.Valor,
                Justificativa = c.Justificativa,
                NomePaciente = c.Paciente.Nome,
                NomeMedico = c.Medico.Nome,
                Medico = new MedicoDTO
                {
                    Id = c.Medico.Id,
                    Nome = c.Medico.Nome,
                    CRM = c.Medico.CRM,
                    Especialidade = c.Medico.Especialidade,
                    ValorConsulta = c.Medico.ValorConsulta,
                    Telefone = c.Medico.Telefone,
                    Email = c.Medico.Email
                },
                Paciente = new PacienteDTO
                {
                    Id = c.Paciente.Id,
                    Nome = c.Paciente.Nome,
                    CPF = c.Paciente.CPF,
                    Email = c.Paciente.Email,
                    Telefone = c.Paciente.Telefone,
                    DataNascimento = c.Paciente.DataNascimento
                }
            });
        }

        public async Task<IEnumerable<ConsultaDTO>> GetByPacienteIdAsync(int pacienteId)
        {
            try
            {
                Console.WriteLine($"Buscando consultas para o paciente {pacienteId}");
                
                var consultas = await _context.Consultas
                    .Include(c => c.Paciente)
                    .Include(c => c.Medico)
                    .Include(c => c.Agenda)
                    .Where(c => c.PacienteId == pacienteId)
                    .ToListAsync();

                Console.WriteLine($"Encontradas {consultas.Count} consultas");

                var result = new List<ConsultaDTO>();
                foreach (var c in consultas)
                {
                    try
                    {
                        if (c == null)
                        {
                            Console.WriteLine("Consulta é nula");
                            continue;
                        }

                        Console.WriteLine($"Processando consulta {c.Id}");
                        Console.WriteLine($"PacienteId: {c.PacienteId}, MedicoId: {c.MedicoId}, AgendaId: {c.AgendaId}");

                        var dto = new ConsultaDTO
                        {
                            Id = c.Id,
                            PacienteId = c.PacienteId,
                            MedicoId = c.MedicoId,
                            AgendaId = c.AgendaId,
                            DataHora = c.DataHora,
                            Status = c.Status,
                            Valor = c.Valor,
                            Justificativa = c.Justificativa,
                            NomePaciente = c.Paciente?.Nome ?? "Não informado",
                            NomeMedico = c.Medico?.Nome ?? "Não informado"
                        };

                        if (c.Medico != null)
                        {
                            dto.Medico = new MedicoDTO
                            {
                                Id = c.Medico.Id,
                                Nome = c.Medico.Nome,
                                CRM = c.Medico.CRM,
                                Especialidade = c.Medico.Especialidade,
                                ValorConsulta = c.Medico.ValorConsulta,
                                Telefone = c.Medico.Telefone,
                                Email = c.Medico.Email
                            };
                        }

                        if (c.Paciente != null)
                        {
                            dto.Paciente = new PacienteDTO
                            {
                                Id = c.Paciente.Id,
                                Nome = c.Paciente.Nome,
                                CPF = c.Paciente.CPF,
                                Email = c.Paciente.Email,
                                Telefone = c.Paciente.Telefone,
                                DataNascimento = c.Paciente.DataNascimento
                            };
                        }

                        result.Add(dto);
                        Console.WriteLine($"Consulta {c.Id} convertida com sucesso");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao converter consulta {c.Id}: {ex.Message}");
                        Console.WriteLine($"Detalhes da consulta: PacienteId={c.PacienteId}, MedicoId={c.MedicoId}, AgendaId={c.AgendaId}");
                    }
                }

                Console.WriteLine($"Retornando {result.Count} consultas convertidas");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar consultas: {ex.Message}");
                throw;
            }
        }

        public async Task<ConsultaDTO> RejeitarConsultaAsync(int id, string justificativa)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Include(c => c.Agenda)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
                return null;

            consulta.Status = StatusConsulta.Recusada;
            consulta.Justificativa = justificativa;

            var agenda = await _context.Agendas.FindAsync(consulta.AgendaId);
            if (agenda != null)
            {
                agenda.Disponivel = true;
                _context.Agendas.Update(agenda);
            }

            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();

            return new ConsultaDTO
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                MedicoId = consulta.MedicoId,
                AgendaId = consulta.AgendaId,
                DataHora = consulta.DataHora,
                Status = consulta.Status,
                Valor = consulta.Valor,
                Justificativa = consulta.Justificativa,
                NomePaciente = consulta.Paciente.Nome,
                NomeMedico = consulta.Medico.Nome,
                Medico = new MedicoDTO
                {
                    Id = consulta.Medico.Id,
                    Nome = consulta.Medico.Nome,
                    CRM = consulta.Medico.CRM,
                    Especialidade = consulta.Medico.Especialidade,
                    ValorConsulta = consulta.Medico.ValorConsulta,
                    Telefone = consulta.Medico.Telefone,
                    Email = consulta.Medico.Email
                },
                Paciente = new PacienteDTO
                {
                    Id = consulta.Paciente.Id,
                    Nome = consulta.Paciente.Nome,
                    CPF = consulta.Paciente.CPF,
                    Email = consulta.Paciente.Email,
                    Telefone = consulta.Paciente.Telefone,
                    DataNascimento = consulta.Paciente.DataNascimento
                }
            };
        }

        public async Task<ConsultaDTO> ConcluirConsultaAsync(int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
                return null;

            consulta.Status = StatusConsulta.Realizada;
            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();

            return new ConsultaDTO
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                MedicoId = consulta.MedicoId,
                AgendaId = consulta.AgendaId,
                DataHora = consulta.DataHora,
                Status = consulta.Status,
                Valor = consulta.Valor,
                Justificativa = consulta.Justificativa,
                NomePaciente = consulta.Paciente.Nome,
                NomeMedico = consulta.Medico.Nome,
                Medico = new MedicoDTO
                {
                    Id = consulta.Medico.Id,
                    Nome = consulta.Medico.Nome,
                    CRM = consulta.Medico.CRM,
                    Especialidade = consulta.Medico.Especialidade,
                    ValorConsulta = consulta.Medico.ValorConsulta,
                    Telefone = consulta.Medico.Telefone,
                    Email = consulta.Medico.Email
                },
                Paciente = new PacienteDTO
                {
                    Id = consulta.Paciente.Id,
                    Nome = consulta.Paciente.Nome,
                    CPF = consulta.Paciente.CPF,
                    Email = consulta.Paciente.Email,
                    Telefone = consulta.Paciente.Telefone,
                    DataNascimento = consulta.Paciente.DataNascimento
                }
            };
        }
    }
}