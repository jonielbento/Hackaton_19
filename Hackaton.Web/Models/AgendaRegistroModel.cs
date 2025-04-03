using System.ComponentModel.DataAnnotations;

namespace Hackaton.Web.Models
{
    public class AgendaRegistroModel
    {
        [Required(ErrorMessage = "O ID do médico é obrigatório")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "A data é obrigatória")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O horário de início é obrigatório")]
        public TimeSpan HorarioInicio { get; set; }

        [Required(ErrorMessage = "O horário de fim é obrigatório")]
        public TimeSpan HorarioFim { get; set; }
    }
} 