using Hackaton.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.Domain.Interfaces
{
    public interface IConsultaRepository
    {
        Task<IEnumerable<Consulta>> GetAllAsync();
        Task<Consulta> GetByIdAsync(int id);
        Task<IEnumerable<Consulta>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Consulta>> GetByPacienteIdAsync(int pacienteId);
        Task<Consulta> CreateAsync(Consulta consulta);
        Task<Consulta> UpdateAsync(Consulta consulta);
        Task<bool> DeleteAsync(int id);
        Task<bool> CancelarConsultaAsync(int id, string justificativa);
    }
}