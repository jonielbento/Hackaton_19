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
    public class PacienteService : IPacienteService
    {
        private readonly HackatonDbContext _context;
        private readonly HashService _hashService;

        public PacienteService(HackatonDbContext context, HashService hashService)
        {
            _context = context;
            _hashService = hashService;
        }

        public async Task<PacienteDTO?> AuthenticateAsync(PacienteLoginDTO loginDTO)
        {
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.CPF == loginDTO.Identificacao || p.Email == loginDTO.Identificacao);

            if (paciente == null)
                return null;

            // Verificar se a senha corresponde
            if (!_hashService.VerifyPassword(loginDTO.Senha, paciente.Senha))
                return null;

            return new PacienteDTO
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                CPF = paciente.CPF,
                Email = paciente.Email,
                Telefone = paciente.Telefone,
                DataNascimento = paciente.DataNascimento
            };
        }

        public async Task<PacienteDTO> CreateAsync(PacienteRegistroDTO pacienteDTO)
        {
            try
            {
                // Verificar se já existe um paciente com o mesmo CPF
                var pacienteExistente = await _context.Pacientes.FirstOrDefaultAsync(p => p.CPF == pacienteDTO.CPF);
                if (pacienteExistente != null)
                    throw new Exception("Já existe um paciente cadastrado com este CPF");

                // Verificar se já existe um paciente com o mesmo email, se fornecido
                if (!string.IsNullOrEmpty(pacienteDTO.Email))
                {
                    var emailExistente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Email == pacienteDTO.Email);
                    if (emailExistente != null)
                        throw new Exception("Já existe um paciente cadastrado com este email");
                }
                
                var paciente = new Paciente
                {
                    Nome = pacienteDTO.Nome,
                    CPF = pacienteDTO.CPF,
                    Senha = _hashService.HashPassword(pacienteDTO.Senha),
                    Email = pacienteDTO.Email,
                    Telefone = pacienteDTO.Telefone,
                    DataNascimento = pacienteDTO.DataNascimento
                };

                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();

                return new PacienteDTO
                {
                    Id = paciente.Id,
                    Nome = paciente.Nome,
                    CPF = paciente.CPF,
                    Email = paciente.Email,
                    Telefone = paciente.Telefone,
                    DataNascimento = paciente.DataNascimento
                };
            }
            catch (Exception ex)
            {
                // Logar o erro
                Console.WriteLine($"Erro ao criar paciente: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                return false;

            // Verificar se o paciente possui consultas
            var possuiConsultas = await _context.Consultas.AnyAsync(c => c.PacienteId == id);

            if (possuiConsultas)
                throw new Exception("Não é possível excluir este paciente pois ele possui consultas cadastradas");

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PacienteDTO>> GetAllAsync()
        {
            var pacientes = await _context.Pacientes.ToListAsync();

            return pacientes.Select(p => new PacienteDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                CPF = p.CPF,
                Email = p.Email,
                Telefone = p.Telefone,
                DataNascimento = p.DataNascimento
            });
        }

        public async Task<PacienteDTO> GetByCPFAsync(string cpf)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.CPF == cpf);

            if (paciente == null)
                return null;

            return new PacienteDTO
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                CPF = paciente.CPF,
                Email = paciente.Email,
                Telefone = paciente.Telefone,
                DataNascimento = paciente.DataNascimento
            };
        }

        public async Task<PacienteDTO> GetByEmailAsync(string email)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Email == email);

            if (paciente == null)
                return null;

            return new PacienteDTO
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                CPF = paciente.CPF,
                Email = paciente.Email,
                Telefone = paciente.Telefone,
                DataNascimento = paciente.DataNascimento
            };
        }

        public async Task<PacienteDTO> GetByIdAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null)
                return null;

            return new PacienteDTO
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                CPF = paciente.CPF,
                Email = paciente.Email,
                Telefone = paciente.Telefone,
                DataNascimento = paciente.DataNascimento
            };
        }

        public async Task<PacienteDTO> UpdateAsync(int id, PacienteDTO pacienteDTO)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                return null;

            // Verificar se o CPF foi alterado e se já existe outro paciente com o mesmo CPF
            if (paciente.CPF != pacienteDTO.CPF)
            {
                var pacienteExistente = await _context.Pacientes.FirstOrDefaultAsync(p => p.CPF == pacienteDTO.CPF && p.Id != id);
                if (pacienteExistente != null)
                    throw new Exception("Já existe um paciente cadastrado com este CPF");
            }

            // Verificar se o email foi alterado e se já existe outro paciente com o mesmo email
            if (!string.IsNullOrEmpty(pacienteDTO.Email) && paciente.Email != pacienteDTO.Email)
            {
                var emailExistente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Email == pacienteDTO.Email && p.Id != id);
                if (emailExistente != null)
                    throw new Exception("Já existe um paciente cadastrado com este email");
            }

            paciente.Nome = pacienteDTO.Nome;
            paciente.CPF = pacienteDTO.CPF;
            paciente.Email = pacienteDTO.Email;
            paciente.Telefone = pacienteDTO.Telefone;
            paciente.DataNascimento = pacienteDTO.DataNascimento;

            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();

            return new PacienteDTO
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                CPF = paciente.CPF,
                Email = paciente.Email,
                Telefone = paciente.Telefone,
                DataNascimento = paciente.DataNascimento
            };
        }
    }
}