using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Hackaton.Domain.Entities;
using Hackaton.Domain.Security;
using Hackaton.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Application.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly HackatonDbContext _context;
        private readonly HashService _hashService;

        public MedicoService(HackatonDbContext context, HashService hashService)
        {
            _context = context;
            _hashService = hashService;
        }

        public async Task<MedicoDTO?> AuthenticateAsync(MedicoLoginDTO loginDTO)
        {
            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.CRM == loginDTO.CRM);

            if (medico == null)
                return null;

            // Verificar se a senha corresponde
            if (!_hashService.VerifyPassword(loginDTO.Senha, medico.Senha))
                return null;

            return new MedicoDTO
            {
                Id = medico.Id,
                Nome = medico.Nome,
                CRM = medico.CRM,
                Especialidade = medico.Especialidade,
                ValorConsulta = medico.ValorConsulta
            };
        }

        public async Task<MedicoDTO> CreateAsync(MedicoRegistroDTO medicoDTO)
        {
            // Verificar se já existe um médico com o mesmo CRM
            var medicoExistente = await _context.Medicos.FirstOrDefaultAsync(m => m.CRM == medicoDTO.CRM);
            if (medicoExistente != null)
                throw new Exception("Já existe um médico cadastrado com este CRM");
            
            var medico = new Medico
            {
                Nome = medicoDTO.Nome,
                CRM = medicoDTO.CRM,
                Senha = _hashService.HashPassword(medicoDTO.Senha),
                Especialidade = medicoDTO.Especialidade,
                ValorConsulta = medicoDTO.ValorConsulta
            };

            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();

            return new MedicoDTO
            {
                Id = medico.Id,
                Nome = medico.Nome,
                CRM = medico.CRM,
                Especialidade = medico.Especialidade,
                ValorConsulta = medico.ValorConsulta
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
                return false;

            // Verificar se o médico possui consultas ou agendas
            var possuiConsultas = await _context.Consultas.AnyAsync(c => c.MedicoId == id);
            var possuiAgendas = await _context.Agendas.AnyAsync(a => a.MedicoId == id);

            if (possuiConsultas || possuiAgendas)
                throw new Exception("Não é possível excluir este médico pois ele possui consultas ou agendas cadastradas");

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MedicoDTO>> GetAllAsync()
        {
            var medicos = await _context.Medicos.ToListAsync();

            return medicos.Select(m => new MedicoDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                CRM = m.CRM,
                Especialidade = m.Especialidade,
                ValorConsulta = m.ValorConsulta
            });
        }

        public async Task<MedicoDTO> GetByCRMAsync(string crm)
        {
            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.CRM == crm);

            if (medico == null)
                return null;

            return new MedicoDTO
            {
                Id = medico.Id,
                Nome = medico.Nome,
                CRM = medico.CRM,
                Especialidade = medico.Especialidade,
                ValorConsulta = medico.ValorConsulta
            };
        }

        public async Task<MedicoDTO> GetByIdAsync(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);

            if (medico == null)
                return null;

            return new MedicoDTO
            {
                Id = medico.Id,
                Nome = medico.Nome,
                CRM = medico.CRM,
                Especialidade = medico.Especialidade,
                ValorConsulta = medico.ValorConsulta
            };
        }

        public async Task<MedicoDTO> UpdateAsync(int id, MedicoDTO medicoDTO)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
                return null;

            // Verificar se o CRM foi alterado e se já existe outro médico com o mesmo CRM
            if (medico.CRM != medicoDTO.CRM)
            {
                var medicoExistente = await _context.Medicos.FirstOrDefaultAsync(m => m.CRM == medicoDTO.CRM && m.Id != id);
                if (medicoExistente != null)
                    throw new Exception("Já existe um médico cadastrado com este CRM");
            }

            medico.Nome = medicoDTO.Nome;
            medico.CRM = medicoDTO.CRM;
            medico.Especialidade = medicoDTO.Especialidade;
            medico.ValorConsulta = medicoDTO.ValorConsulta;

            _context.Medicos.Update(medico);
            await _context.SaveChangesAsync();

            return new MedicoDTO
            {
                Id = medico.Id,
                Nome = medico.Nome,
                CRM = medico.CRM,
                Especialidade = medico.Especialidade,
                ValorConsulta = medico.ValorConsulta
            };
        }

        public async Task<IEnumerable<MedicoDTO>> GetByEspecialidadeAsync(string especialidade)
        {
            var medicos = await _context.Medicos
                .Where(m => m.Especialidade.ToLower().Contains(especialidade.ToLower()))
                .ToListAsync();

            return medicos.Select(m => new MedicoDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                CRM = m.CRM,
                Especialidade = m.Especialidade,
                ValorConsulta = m.ValorConsulta
            });
        }
    }
}