using Hackaton.Domain.Entities;
using Hackaton.Domain.Interfaces;
using Hackaton.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Infrastructure.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly HackatonDbContext _context;

        public ConsultaRepository(HackatonDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CancelarConsultaAsync(int id, string justificativa)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
                return false;

            consulta.Status = StatusConsulta.Cancelada;
            consulta.Justificativa = justificativa;

            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Consulta> CreateAsync(Consulta consulta)
        {
            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();
            return consulta;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
                return false;

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Consulta>> GetAllAsync()
        {
            return await _context.Consultas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .Include(c => c.Agenda)
                .ToListAsync();
        }

        public async Task<Consulta> GetByIdAsync(int id)
        {
            return await _context.Consultas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .Include(c => c.Agenda)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Consulta>> GetByMedicoIdAsync(int medicoId)
        {
            return await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Agenda)
                .Where(c => c.MedicoId == medicoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteIdAsync(int pacienteId)
        {
            return await _context.Consultas
                .Include(c => c.Medico)
                .Include(c => c.Agenda)
                .Where(c => c.PacienteId == pacienteId)
                .ToListAsync();
        }

        public async Task<Consulta> UpdateAsync(Consulta consulta)
        {
            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();
            return consulta;
        }
    }
}