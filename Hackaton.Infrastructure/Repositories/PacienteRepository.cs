using Hackaton.Domain.Entities;
using Hackaton.Domain.Interfaces;
using Hackaton.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Infrastructure.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly HackatonDbContext _context;

        public PacienteRepository(HackatonDbContext context)
        {
            _context = context;
        }

        public async Task<Paciente> AuthenticateAsync(string cpf, string senha)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.CPF == cpf && p.Senha == senha);
        }

        public async Task<Paciente> CreateAsync(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            return paciente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                return false;

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }

        public async Task<Paciente> GetByCPFAsync(string cpf)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.CPF == cpf);
        }

        public async Task<Paciente> GetByIdAsync(int id)
        {
            return await _context.Pacientes.FindAsync(id);
        }

        public async Task<Paciente> UpdateAsync(Paciente paciente)
        {
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();
            return paciente;
        }
    }
}