using Hackaton.Domain.Entities;
using Hackaton.Domain.Security;
using Hackaton.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly HackatonDbContext _context;
        private readonly HashService _hashService;

        public AdminController(HackatonDbContext context, HashService hashService)
        {
            _context = context;
            _hashService = hashService;
        }

        [HttpPost("hash-senhas")]
        public async Task<IActionResult> HashSenhasExistentes()
        {
            try
            {
                // Obter todos os médicos
                var medicos = await _context.Medicos.ToListAsync();
                int medicoCount = 0;

                // Atualizar senhas de médicos
                foreach (var medico in medicos)
                {
                    // Verificar se a senha já é um hash (assumindo que hashes têm comprimento fixo de acordo com SHA-256)
                    if (!IsPossibleHash(medico.Senha))
                    {
                        var senhaOriginal = medico.Senha;
                        medico.Senha = _hashService.HashPassword(senhaOriginal);
                        medicoCount++;
                    }
                }

                // Obter todos os pacientes
                var pacientes = await _context.Pacientes.ToListAsync();
                int pacienteCount = 0;

                // Atualizar senhas de pacientes
                foreach (var paciente in pacientes)
                {
                    // Verificar se a senha já é um hash
                    if (!IsPossibleHash(paciente.Senha))
                    {
                        var senhaOriginal = paciente.Senha;
                        paciente.Senha = _hashService.HashPassword(senhaOriginal);
                        pacienteCount++;
                    }
                }

                // Salvar alterações
                await _context.SaveChangesAsync();

                return Ok(new 
                { 
                    message = "Senhas atualizadas com sucesso", 
                    medicosAtualizados = medicoCount,
                    pacientesAtualizados = pacienteCount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar senhas: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao atualizar senhas: " + ex.Message });
            }
        }

        [HttpPost("redefinir-senha-medico/{crm}")]
        public async Task<IActionResult> RedefinirSenhaMedico(string crm, [FromBody] RedefinirSenhaRequest request)
        {
            try
            {
                var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.CRM == crm);
                if (medico == null)
                    return NotFound(new { message = $"Médico com CRM {crm} não encontrado" });

                medico.Senha = _hashService.HashPassword(request.NovaSenha);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Senha redefinida com sucesso" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao redefinir senha: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao redefinir senha: " + ex.Message });
            }
        }

        [HttpPost("redefinir-senha-paciente/{cpf}")]
        public async Task<IActionResult> RedefinirSenhaPaciente(string cpf, [FromBody] RedefinirSenhaRequest request)
        {
            try
            {
                var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.CPF == cpf);
                if (paciente == null)
                    return NotFound(new { message = $"Paciente com CPF {cpf} não encontrado" });

                paciente.Senha = _hashService.HashPassword(request.NovaSenha);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Senha redefinida com sucesso" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao redefinir senha: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao redefinir senha: " + ex.Message });
            }
        }

        // Método auxiliar para verificar se uma string já é possivelmente um hash
        private bool IsPossibleHash(string password)
        {
            // Hashes SHA-256 em Base64 têm normalmente comprimento específico e contêm caracteres específicos
            return password.Length >= 40 && (password.Contains("+") || password.Contains("/") || password.Contains("="));
        }
    }

    public class RedefinirSenhaRequest
    {
        public string NovaSenha { get; set; } = string.Empty;
    }
} 