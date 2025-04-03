using Hackaton.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.Domain.Interfaces
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente> GetByIdAsync(int id);
        Task<Paciente> GetByCPFAsync(string cpf);
        Task<Paciente> CreateAsync(Paciente paciente);
        Task<Paciente> UpdateAsync(Paciente paciente);
        Task<bool> DeleteAsync(int id);
        Task<Paciente> AuthenticateAsync(string cpf, string senha);
    }
}