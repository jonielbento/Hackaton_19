using Hackaton.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.Application.Interfaces
{
    public interface IAgendaService
    {
        Task<IEnumerable<AgendaDTO>> GetAllAsync();
        Task<AgendaDTO> GetByIdAsync(int id);
        Task<IEnumerable<AgendaDTO>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<AgendaDTO>> GetDisponiveisByMedicoIdAsync(int medicoId);
        Task<IEnumerable<AgendaDTO>> GetByDataAsync(DateTime data);
        Task<AgendaDTO> CreateAsync(AgendaRegistroDTO agendaDTO);
        Task<AgendaDTO> UpdateAsync(int id, AgendaDTO agendaDTO);
        Task<bool> DeleteAsync(int id);
        Task<bool> MarcarIndisponivel(int id);
    }
}