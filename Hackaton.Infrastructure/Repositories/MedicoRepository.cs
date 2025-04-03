using Hackaton.Domain.Entities;
using Hackaton.Domain.Interfaces;
using Hackaton.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Infrastructure.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly HackatonDbContext _context;

        public MedicoRepository(HackatonDbContext context)
        {
            _context = context;
        }

        public async Task<Medico> AuthenticateAsync(string crm, string senha)
        {
            return await _context.Medicos
                .FirstOrDefaultAsync(m => m.CRM == crm && m.Senha == senha);
        }

        public async Task<Medico> CreateAsync(Medico medico)
        {
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
            return medico;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
                return false;

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Medico>> GetAllAsync()
        {
            return await _context.Medicos.ToListAsync();
        }

        public async Task<Medico> GetByCRMAsync(string crm)
        {
            return await _context.Medicos
                .FirstOrDefaultAsync(m => m.CRM == crm);
        }

        public async Task<Medico> GetByIdAsync(int id)
        {
            return await _context.Medicos.FindAsync(id);
        }

        public async Task<Medico> UpdateAsync(Medico medico)
        {
            _context.Medicos.Update(medico);
            await _context.SaveChangesAsync();
            return medico;
        }
    }
}