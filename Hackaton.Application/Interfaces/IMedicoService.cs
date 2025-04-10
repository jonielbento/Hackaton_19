using Hackaton.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.Application.Interfaces
{
    public interface IMedicoService
    {
        Task<IEnumerable<MedicoDTO>> GetAllAsync();
        Task<MedicoDTO> GetByIdAsync(int id);
        Task<MedicoDTO> GetByCRMAsync(string crm);
        Task<IEnumerable<MedicoDTO>> GetByEspecialidadeAsync(string especialidade);
        Task<MedicoDTO> CreateAsync(MedicoRegistroDTO medicoDTO);
        Task<MedicoDTO> UpdateAsync(int id, MedicoDTO medicoDTO);
        Task<bool> DeleteAsync(int id);
        Task<MedicoDTO> AuthenticateAsync(MedicoLoginDTO loginDTO);
    }
}