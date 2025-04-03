using Hackaton.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.Domain.Interfaces
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<Medico>> GetAllAsync();
        Task<Medico> GetByIdAsync(int id);
        Task<Medico> GetByCRMAsync(string crm);
        Task<Medico> CreateAsync(Medico medico);
        Task<Medico> UpdateAsync(Medico medico);
        Task<bool> DeleteAsync(int id);
        Task<Medico> AuthenticateAsync(string crm, string senha);
    }
}