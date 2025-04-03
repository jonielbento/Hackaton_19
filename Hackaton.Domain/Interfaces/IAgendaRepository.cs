using Hackaton.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.Domain.Interfaces
{
    public interface IAgendaRepository
    {
        Task<IEnumerable<Agenda>> GetAllAsync();
        Task<Agenda> GetByIdAsync(int id);
        Task<Agenda> GetAgendaComMedicoByIdAsync(int id);
        Task<IEnumerable<Agenda>> GetByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Agenda>> GetDisponiveisByMedicoIdAsync(int medicoId);
        Task<IEnumerable<Agenda>> GetByDataAsync(DateTime data);
        Task<Agenda> CreateAsync(Agenda agenda);
        Task<Agenda> UpdateAsync(Agenda agenda);
        Task<bool> DeleteAsync(int id);
        Task<bool> MarcarIndisponivelAsync(int id);
        Task<bool> VerificarConflitoHorarioAsync(int medicoId, DateTime dataHoraInicio, DateTime dataHoraFim);
        Task<bool> VerificarAgendaAssociadaConsultaAsync(int agendaId);
    }
}