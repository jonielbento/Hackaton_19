using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hackaton.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMedicoService _medicoService;
        private readonly IPacienteService _pacienteService;

        public AuthController(IMedicoService medicoService, IPacienteService pacienteService)
        {
            _medicoService = medicoService;
            _pacienteService = pacienteService;
        }

        [HttpPost("medico/login")]
        public async Task<ActionResult<AuthResponseDTO>> LoginMedico([FromBody] MedicoLoginDTO loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.CRM) || string.IsNullOrEmpty(loginDTO.Senha))
                    return BadRequest("CRM e senha são obrigatórios");

                var medico = await _medicoService.AuthenticateAsync(loginDTO);
                if (medico == null)
                    return Unauthorized("CRM ou senha inválidos");

                return Ok(new AuthResponseDTO
                {
                    UserType = "Medico",
                    UserId = medico.Id,
                    Nome = medico.Nome
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("paciente/login")]
        public async Task<ActionResult<AuthResponseDTO>> LoginPaciente([FromBody] PacienteLoginDTO loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.Identificacao) || string.IsNullOrEmpty(loginDTO.Senha))
                    return BadRequest("CPF/Email e senha são obrigatórios");

                var paciente = await _pacienteService.AuthenticateAsync(loginDTO);
                if (paciente == null)
                    return Unauthorized("CPF/Email ou senha inválidos");

                return Ok(new AuthResponseDTO
                {
                    UserType = "Paciente",
                    UserId = paciente.Id,
                    Nome = paciente.Nome
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}