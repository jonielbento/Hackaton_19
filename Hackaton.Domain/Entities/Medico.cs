using System;
using System.Collections.Generic;

namespace Hackaton.Domain.Entities
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string CRM { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string Especialidade { get; set; } = null!;
        public decimal ValorConsulta { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        
        // Relacionamentos
        public virtual ICollection<Agenda> Agendas { get; set; } = new List<Agenda>();
        public virtual ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}