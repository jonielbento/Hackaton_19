using System;

namespace Hackaton.Application.DTOs
{
    public class PacienteDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string CPF { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
    }
    
    public class PacienteRegistroDTO
    {
        public string Nome { get; set; } = null!;
        public string CPF { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}