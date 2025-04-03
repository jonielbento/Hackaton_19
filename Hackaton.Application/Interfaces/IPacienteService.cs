using Hackaton.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.Application.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteDTO>> GetAllAsync();
        Task<PacienteDTO> GetByIdAsync(int id);
        Task<PacienteDTO> GetByCPFAsync(string cpf);
        Task<PacienteDTO> GetByEmailAsync(string email);
        Task<PacienteDTO> CreateAsync(PacienteRegistroDTO pacienteDTO);
        Task<PacienteDTO> UpdateAsync(int id, PacienteDTO pacienteDTO);
        Task<bool> DeleteAsync(int id);
        Task<PacienteDTO> AuthenticateAsync(PacienteLoginDTO loginDTO);
    }
}