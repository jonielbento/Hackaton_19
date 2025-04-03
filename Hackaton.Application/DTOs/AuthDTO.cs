using System;

namespace Hackaton.Application.DTOs
{
    public class AuthResponseDTO
    {
        public string UserType { get; set; } = null!;
        public int UserId { get; set; }
        public string Nome { get; set; } = null!;
    }

    public class MedicoLoginDTO
    {
        public string CRM { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }

    public class PacienteLoginDTO
    {
        public string Identificacao { get; set; } = null!; // CPF ou Email
        public string Senha { get; set; } = null!;
    }
}