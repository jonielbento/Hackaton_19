using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Hackaton.Domain.Entities;
using Hackaton.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Application.Services
{
    public class AgendaService : IAgendaService
    {
        private readonly IAgendaRepository _agendaRepository;
        private readonly IMedicoRepository _medicoRepository;
        private readonly IConsultaRepository _consultaRepository;

        public AgendaService(IAgendaRepository agendaRepository, IMedicoRepository medicoRepository, IConsultaRepository consultaRepository)
        {
            _agendaRepository = agendaRepository;
            _medicoRepository = medicoRepository;
            _consultaRepository = consultaRepository;
        }

        public async Task<AgendaDTO> CreateAsync(AgendaRegistroDTO agendaDTO)
        {
            var medico = await _medicoRepository.GetByIdAsync(agendaDTO.MedicoId);
            if (medico == null)
                throw new Exception("Médico não encontrado");

            // Verificar se já existe um horário conflitante para este médico
            var existeConflito = await _agendaRepository.VerificarConflitoHorarioAsync(
                agendaDTO.MedicoId, 
                agendaDTO.DataHoraInicio, 
                agendaDTO.DataHoraFim);

            if (existeConflito)
                throw new Exception("Já existe um horário cadastrado que conflita com este período");

            var agenda = new Agenda
            {
                MedicoId = agendaDTO.MedicoId,
                DataHoraInicio = agendaDTO.DataHoraInicio,
                DataHoraFim = agendaDTO.DataHoraFim,
                Disponivel = true
            };

            agenda = await _agendaRepository.CreateAsync(agenda);

            return new AgendaDTO
            {
                Id = agenda.Id,
                MedicoId = agenda.MedicoId,
                DataHoraInicio = agenda.DataHoraInicio,
                DataHoraFim = agenda.DataHoraFim,
                Disponivel = agenda.Disponivel,
                NomeMedico = medico.Nome
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var agenda = await _agendaRepository.GetByIdAsync(id);
            if (agenda == null)
                return false;

            // Verificar se a agenda está associada a alguma consulta
            var agendaAssociadaConsulta = await _agendaRepository.VerificarAgendaAssociadaConsultaAsync(id);
            if (agendaAssociadaConsulta)
                throw new Exception("Não é possível excluir este horário pois ele está associado a uma consulta");

            return await _agendaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<AgendaDTO>> GetAllAsync()
        {
            var agendas = await _agendaRepository.GetAllAsync();

            return agendas.Select(a => new AgendaDTO
            {
                Id = a.Id,
                MedicoId = a.MedicoId,
                DataHoraInicio = a.DataHoraInicio,
                DataHoraFim = a.DataHoraFim,
                Disponivel = a.Disponivel,
                NomeMedico = a.Medico?.Nome ?? "Desconhecido"
            });
        }

        public async Task<IEnumerable<AgendaDTO>> GetByDataAsync(DateTime data)
        {
            var agendas = await _agendaRepository.GetByDataAsync(data);

            return agendas.Select(a => new AgendaDTO
            {
                Id = a.Id,
                MedicoId = a.MedicoId,
                DataHoraInicio = a.DataHoraInicio,
                DataHoraFim = a.DataHoraFim,
                Disponivel = a.Disponivel,
                NomeMedico = a.Medico?.Nome ?? "Desconhecido"
            });
        }

        public async Task<AgendaDTO> GetByIdAsync(int id)
        {
            var agenda = await _agendaRepository.GetAgendaComMedicoByIdAsync(id);

            if (agenda == null)
                return null;

            return new AgendaDTO
            {
                Id = agenda.Id,
                MedicoId = agenda.MedicoId,
                DataHoraInicio = agenda.DataHoraInicio,
                DataHoraFim = agenda.DataHoraFim,
                Disponivel = agenda.Disponivel,
                NomeMedico = agenda.Medico?.Nome ?? "Desconhecido"
            };
        }
        public async Task<IEnumerable<AgendaDTO>> GetByMedicoIdAsync(int medicoId)
        {
            var agendas = await _agendaRepository.GetByMedicoIdAsync(medicoId);

            return agendas.Select(a => new AgendaDTO
            {
                Id = a.Id,
                MedicoId = a.MedicoId,
                DataHoraInicio = a.DataHoraInicio,
                DataHoraFim = a.DataHoraFim,
                Disponivel = a.Disponivel,
                NomeMedico = a.Medico?.Nome ?? "Desconhecido"
            });
        }

        public async Task<IEnumerable<AgendaDTO>> GetDisponiveisByMedicoIdAsync(int medicoId)
        {
            var agendas = await _agendaRepository.GetDisponiveisByMedicoIdAsync(medicoId);

            return agendas.Select(a => new AgendaDTO
            {
                Id = a.Id,
                MedicoId = a.MedicoId,
                DataHoraInicio = a.DataHoraInicio,
                DataHoraFim = a.DataHoraFim,
                Disponivel = a.Disponivel,
                NomeMedico = a.Medico?.Nome ?? "Desconhecido"
            });
        }

        public async Task<bool> MarcarIndisponivel(int id)
        {
            return await _agendaRepository.MarcarIndisponivelAsync(id);
        }

        public async Task<AgendaDTO> UpdateAsync(int id, AgendaDTO agendaDTO)
        {
            var agenda = await _agendaRepository.GetByIdAsync(id);
            if (agenda == null)
                return null;

            // Verificar se a agenda está associada a alguma consulta
            var agendaAssociadaConsulta = await _agendaRepository.VerificarAgendaAssociadaConsultaAsync(id);
            if (agendaAssociadaConsulta && agenda.Disponivel != agendaDTO.Disponivel)
                throw new Exception("Não é possível alterar a disponibilidade deste horário pois ele está associado a uma consulta");

            agenda.DataHoraInicio = agendaDTO.DataHoraInicio;
            agenda.DataHoraFim = agendaDTO.DataHoraFim;
            agenda.Disponivel = agendaDTO.Disponivel;

            agenda = await _agendaRepository.UpdateAsync(agenda);
            var medico = await _medicoRepository.GetByIdAsync(agenda.MedicoId);

            return new AgendaDTO
            {
                Id = agenda.Id,
                MedicoId = agenda.MedicoId,
                DataHoraInicio = agenda.DataHoraInicio,
                DataHoraFim = agenda.DataHoraFim,
                Disponivel = agenda.Disponivel,
                NomeMedico = medico?.Nome ?? "Desconhecido"
            };
        }
    }
}