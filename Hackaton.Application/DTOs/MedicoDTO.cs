using System;

namespace Hackaton.Application.DTOs
{
    public class MedicoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string CRM { get; set; } = null!;
        public string Especialidade { get; set; } = null!;
        public decimal ValorConsulta { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
    
    public class MedicoRegistroDTO
    {
        public string Nome { get; set; } = null!;
        public string CRM { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string Especialidade { get; set; } = null!;
        public decimal ValorConsulta { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
}