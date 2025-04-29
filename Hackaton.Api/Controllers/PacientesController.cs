using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteDTO>>> GetAll()
        {
            try
            {
                var pacientes = await _pacienteService.GetAllAsync();
                return Ok(pacientes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar pacientes: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao buscar pacientes: " + ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteDTO>> GetById(int id)
        {
            try
            {
                var paciente = await _pacienteService.GetByIdAsync(id);
                if (paciente == null)
                    return NotFound(new { message = $"Paciente com ID {id} não encontrado" });

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar paciente por ID: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao buscar paciente: " + ex.Message });
            }
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<ActionResult<PacienteDTO>> GetByCPF(string cpf)
        {
            try
            {
                var paciente = await _pacienteService.GetByCPFAsync(cpf);
                if (paciente == null)
                    return NotFound(new { message = $"Paciente com CPF {cpf} não encontrado" });

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar paciente por CPF: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao buscar paciente: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<PacienteDTO>> Create(PacienteRegistroDTO pacienteDTO)
        {
            try
            {
                Console.WriteLine($"Iniciando cadastro de paciente: {pacienteDTO.Nome}, CPF: {pacienteDTO.CPF}");
                var paciente = await _pacienteService.CreateAsync(pacienteDTO);
                Console.WriteLine($"Paciente cadastrado com sucesso! ID: {paciente.Id}");
                return CreatedAtAction(nameof(GetById), new { id = paciente.Id }, paciente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar paciente: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PacienteDTO>> Update(int id, PacienteDTO pacienteDTO)
        {
            if (id != pacienteDTO.Id)
                return BadRequest(new { message = "ID na URL não corresponde ao ID no corpo da requisição" });

            try
            {
                var paciente = await _pacienteService.UpdateAsync(id, pacienteDTO);
                if (paciente == null)
                    return NotFound(new { message = $"Paciente com ID {id} não encontrado" });

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar paciente: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _pacienteService.DeleteAsync(id);
                if (!result)
                    return NotFound(new { message = $"Paciente com ID {id} não encontrado" });

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir paciente: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<PacienteDTO>> Login(PacienteLoginDTO loginDTO)
        {
            var paciente = await _pacienteService.AuthenticateAsync(loginDTO);
            if (paciente == null)
                return Unauthorized("CPF/Email ou senha inválidos");

            return Ok(paciente);
        }
    }
}