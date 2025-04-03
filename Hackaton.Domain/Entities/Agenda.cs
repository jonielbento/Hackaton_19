using System;

namespace Hackaton.Domain.Entities
{
    public class Agenda
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public bool Disponivel { get; set; } = true;
        
        // Relacionamentos
        public virtual Medico Medico { get; set; } = null!;
    }
}