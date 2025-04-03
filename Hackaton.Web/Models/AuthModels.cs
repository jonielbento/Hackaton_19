using System;

namespace Hackaton.Web.Models
{
    public class AuthResponseModel
    {
        public string UserType { get; set; } = null!;
        public int UserId { get; set; }
        public string Nome { get; set; } = null!;
    }

    public class MedicoLoginModel
    {
        public string CRM { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }

    public class PacienteLoginModel
    {
        public string Identificacao { get; set; } = null!; // CPF ou Email
        public string Senha { get; set; } = null!;
    }

    public class UserSession
    {
        public string UserType { get; set; } = null!;
        public int UserId { get; set; }
        public string Nome { get; set; } = null!;
        public bool IsAuthenticated => !string.IsNullOrEmpty(UserType);
    }
}