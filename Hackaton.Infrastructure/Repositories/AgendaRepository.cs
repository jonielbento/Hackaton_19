using Hackaton.Domain.Entities;
using Hackaton.Domain.Interfaces;
using Hackaton.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Infrastructure.Repositories
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly HackatonDbContext _context;

        public AgendaRepository(HackatonDbContext context)
        {
            _context = context;
        }

        public async Task<Agenda> CreateAsync(Agenda agenda)
        {
            _context.Agendas.Add(agenda);
            await _context.SaveChangesAsync();
            return agenda;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var agenda = await _context.Agendas.FindAsync(id);
            if (agenda == null)
                return false;

            _context.Agendas.Remove(agenda);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Agenda>> GetAllAsync()
        {
            return await _context.Agendas
                .Include(a => a.Medico)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agenda>> GetByDataAsync(DateTime data)
        {
            var dataInicio = data.Date;
            var dataFim = dataInicio.AddDays(1).AddTicks(-1);

            return await _context.Agendas
                .Include(a => a.Medico)
                .Where(a => a.DataHoraInicio >= dataInicio && a.DataHoraInicio <= dataFim)
                .ToListAsync();
        }

        public async Task<Agenda> GetByIdAsync(int id)
        {
            return await _context.Agendas.FindAsync(id);
        }

        public async Task<Agenda> GetAgendaComMedicoByIdAsync(int id)
        {
            return await _context.Agendas
                .Include(a => a.Medico)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Agenda>> GetByMedicoIdAsync(int medicoId)
        {
            return await _context.Agendas
                .Include(a => a.Medico)
                .Where(a => a.MedicoId == medicoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agenda>> GetDisponiveisByMedicoIdAsync(int medicoId)
        {
            var agora = DateTime.Now;
            var agendas = await _context.Agendas
                .Include(a => a.Medico)
                .Where(a => a.MedicoId == medicoId && 
                           a.Disponivel && 
                           a.DataHoraInicio.Date >= agora.Date &&
                           !_context.Consultas.Any(c => c.AgendaId == a.Id && c.Status != StatusConsulta.Cancelada && c.Status != StatusConsulta.Recusada))
                .OrderBy(a => a.DataHoraInicio)
                .ToListAsync();

            Console.WriteLine($"Encontradas {agendas.Count()} agendas disponíveis para o médico {medicoId}");
            foreach (var agenda in agendas)
            {
                Console.WriteLine($"Agenda {agenda.Id}: {agenda.DataHoraInicio} - {agenda.DataHoraFim}");
            }

            return agendas;
        }

        public async Task<bool> MarcarIndisponivelAsync(int id)
        {
            var agenda = await _context.Agendas.FindAsync(id);
            if (agenda == null)
                return false;

            agenda.Disponivel = false;
            _context.Agendas.Update(agenda);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Agenda> UpdateAsync(Agenda agenda)
        {
            _context.Agendas.Update(agenda);
            await _context.SaveChangesAsync();
            return agenda;
        }

        public async Task<bool> VerificarConflitoHorarioAsync(int medicoId, DateTime dataHoraInicio, DateTime dataHoraFim)
        {
            var conflito = await _context.Agendas
                .Where(a => a.MedicoId == medicoId)
                .Where(a => (a.DataHoraInicio <= dataHoraInicio && a.DataHoraFim > dataHoraInicio) ||
                           (a.DataHoraInicio < dataHoraFim && a.DataHoraFim >= dataHoraFim) ||
                           (a.DataHoraInicio >= dataHoraInicio && a.DataHoraFim <= dataHoraFim))
                .FirstOrDefaultAsync();

            return conflito != null;
        }

        public async Task<bool> VerificarAgendaAssociadaConsultaAsync(int agendaId)
        {
            var consulta = await _context.Consultas.FirstOrDefaultAsync(c => c.AgendaId == agendaId);
            return consulta != null;
        }
    }
}