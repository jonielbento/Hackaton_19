using System.ComponentModel.DataAnnotations;

namespace Hackaton.Web.Models
{
    public class MedicoRegistroModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CRM é obrigatório")]
        [StringLength(20, ErrorMessage = "O CRM deve ter no máximo 20 caracteres")]
        public string CRM { get; set; } = string.Empty;

        [Required(ErrorMessage = "A especialidade é obrigatória")]
        [StringLength(50, ErrorMessage = "A especialidade deve ter no máximo 50 caracteres")]
        public string Especialidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O valor da consulta é obrigatório")]
        [Range(0.01, 9999.99, ErrorMessage = "O valor da consulta deve ser maior que zero e menor que 10.000")]
        public decimal ValorConsulta { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
} 