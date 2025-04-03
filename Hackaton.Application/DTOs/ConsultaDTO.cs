using System;
using Hackaton.Domain.Entities;

namespace Hackaton.Application.DTOs
{
    public class ConsultaDTO
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int AgendaId { get; set; }
        public DateTime DataHora { get; set; }
        public StatusConsulta Status { get; set; }
        public decimal Valor { get; set; }
        public string? Justificativa { get; set; }
        public string NomePaciente { get; set; } = string.Empty;
        public string NomeMedico { get; set; } = string.Empty;
        public MedicoDTO? Medico { get; set; }
        public PacienteDTO? Paciente { get; set; }
    }

    public class ConsultaRegistroDTO
    {
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int AgendaId { get; set; }
        public DateTime DataHora { get; set; }
    }
    
    public class ConsultaAtualizacaoDTO
    {
        public int Id { get; set; }
        public StatusConsulta Status { get; set; }
        public string? Justificativa { get; set; }
    }
}