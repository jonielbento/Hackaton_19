using System;

namespace Hackaton.Domain.Entities
{
    public enum StatusConsulta
    {
        Agendada,
        Confirmada,
        Recusada,
        Cancelada,
        Realizada
    }
    
    public class Consulta
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int AgendaId { get; set; }
        public DateTime DataHora { get; set; }
        public StatusConsulta Status { get; set; } = StatusConsulta.Agendada;
        public decimal Valor { get; set; }
        public string? Justificativa { get; set; }
        
        // Relacionamentos
        public virtual Paciente Paciente { get; set; } = null!;
        public virtual Medico Medico { get; set; } = null!;
        public virtual Agenda Agenda { get; set; } = null!;
    }
}