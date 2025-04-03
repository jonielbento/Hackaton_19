using System;
using System.Collections.Generic;

namespace Hackaton.Domain.Entities
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string CPF { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        
        // Relacionamentos
        public virtual ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}