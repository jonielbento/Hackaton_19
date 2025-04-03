using System;

namespace Hackaton.Application.DTOs
{
    public class AgendaDTO
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public bool Disponivel { get; set; }
        public string NomeMedico { get; set; } = null!;
    }
    
    public class AgendaRegistroDTO
    {
        public int MedicoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
    }
}