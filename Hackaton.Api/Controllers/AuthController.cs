using Hackaton.Api.Security;
using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hackaton.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMedicoService _medicoService;
        private readonly IPacienteService _pacienteService;
        private readonly JwtService _jwtService;

        public AuthController(
            IMedicoService medicoService, 
            IPacienteService pacienteService, 
            JwtService jwtService)
        {
            _medicoService = medicoService;
            _pacienteService = pacienteService;
            _jwtService = jwtService;
        }

        [HttpPost("medico/login")]
        public async Task<IActionResult> LoginMedico([FromBody] MedicoLoginDTO loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.CRM) || string.IsNullOrEmpty(loginDTO.Senha))
                    return BadRequest(new { message = "CRM e senha são obrigatórios" });

                var medico = await _medicoService.AuthenticateAsync(loginDTO);
                
                if (medico == null)
                    return Unauthorized(new { message = "CRM ou senha inválidos" });

                // Gerar token JWT
                var token = _jwtService.GenerateJwtToken(medico.Id, medico.Nome, "Medico");
                var refreshToken = _jwtService.GenerateRefreshToken();
                var refreshTokenExpiryTime = _jwtService.GetRefreshTokenExpiryTime();

                return Ok(new
                {
                    userId = medico.Id,
                    nome = medico.Nome,
                    userType = "Medico",
                    token,
                    refreshToken,
                    refreshTokenExpiryTime
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao autenticar médico: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao autenticar: " + ex.Message });
            }
        }

        [HttpPost("paciente/login")]
        public async Task<IActionResult> LoginPaciente([FromBody] PacienteLoginDTO loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.Identificacao) || string.IsNullOrEmpty(loginDTO.Senha))
                    return BadRequest(new { message = "Identificação e senha são obrigatórios" });

                var paciente = await _pacienteService.AuthenticateAsync(loginDTO);
                
                if (paciente == null)
                    return Unauthorized(new { message = "Identificação ou senha inválidos" });

                // Gerar token JWT
                var token = _jwtService.GenerateJwtToken(paciente.Id, paciente.Nome, "Paciente");
                var refreshToken = _jwtService.GenerateRefreshToken();
                var refreshTokenExpiryTime = _jwtService.GetRefreshTokenExpiryTime();

                return Ok(new
                {
                    userId = paciente.Id,
                    nome = paciente.Nome,
                    userType = "Paciente",
                    token,
                    refreshToken,
                    refreshTokenExpiryTime
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao autenticar paciente: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao autenticar: " + ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshRequest.RefreshToken) || string.IsNullOrEmpty(refreshRequest.Token))
                {
                    return BadRequest(new { message = "Token e Refresh Token são necessários" });
                }

                if (!_jwtService.ValidateToken(refreshRequest.Token, out var jwtToken))
                {
                    return Unauthorized(new { message = "Token inválido" });
                }

                // Aqui, em uma implementação completa, você verificaria se o refresh token está válido no banco de dados
                // Para simplificar, estamos apenas gerando um novo token

                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                var userName = jwtToken.Claims.First(x => x.Type == System.Security.Claims.ClaimTypes.Name).Value;
                var userRole = jwtToken.Claims.First(x => x.Type == System.Security.Claims.ClaimTypes.Role).Value;

                // Gerar novo token JWT
                var newToken = _jwtService.GenerateJwtToken(userId, userName, userRole);
                var newRefreshToken = _jwtService.GenerateRefreshToken();
                var refreshTokenExpiryTime = _jwtService.GetRefreshTokenExpiryTime();

                return Ok(new
                {
                    token = newToken,
                    refreshToken = newRefreshToken,
                    refreshTokenExpiryTime
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao renovar token: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao renovar token: " + ex.Message });
            }
        }
    }

    public class RefreshTokenRequest
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}