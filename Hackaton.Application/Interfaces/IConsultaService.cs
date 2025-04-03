using Hackaton.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.Application.Interfaces
{
    public interface IConsultaService
    {
        Task<IEnumerable<ConsultaDTO>> GetAllAsync();
        Task<ConsultaDTO> GetByIdAsync(int id);
        Task<IEnumerable<ConsultaDTO>> GetByPacienteIdAsync(int pacienteId);
        Task<IEnumerable<ConsultaDTO>> GetByMedicoIdAsync(int medicoId);
        Task<ConsultaDTO> CreateAsync(ConsultaRegistroDTO consultaDTO);
        Task<bool> DeleteAsync(int id);
        Task<ConsultaDTO> AceitarConsultaAsync(int id);
        Task<ConsultaDTO> CancelarConsultaAsync(int id, string justificativa);
        Task<ConsultaDTO> RejeitarConsultaAsync(int id, string justificativa);
        Task<ConsultaDTO> ConcluirConsultaAsync(int id);
    }
}