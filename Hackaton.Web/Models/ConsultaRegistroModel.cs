using System.ComponentModel.DataAnnotations;

namespace Hackaton.Web.Models
{
    public class ConsultaRegistroModel
    {
        [Required(ErrorMessage = "O ID do médico é obrigatório")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "O ID do paciente é obrigatório")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "O ID da agenda é obrigatório")]
        public int AgendaId { get; set; }

        [Required(ErrorMessage = "A data e hora são obrigatórias")]
        public DateTime DataHora { get; set; }
    }
} 