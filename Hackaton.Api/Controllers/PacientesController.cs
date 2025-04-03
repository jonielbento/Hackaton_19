using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            var pacientes = await _pacienteService.GetAllAsync();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteDTO>> GetById(int id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<ActionResult<PacienteDTO>> GetByCPF(string cpf)
        {
            var paciente = await _pacienteService.GetByCPFAsync(cpf);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpPost]
        public async Task<ActionResult<PacienteDTO>> Create(PacienteRegistroDTO pacienteDTO)
        {
            try
            {
                var paciente = await _pacienteService.CreateAsync(pacienteDTO);
                return CreatedAtAction(nameof(GetById), new { id = paciente.Id }, paciente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PacienteDTO>> Update(int id, PacienteDTO pacienteDTO)
        {
            if (id != pacienteDTO.Id)
                return BadRequest("ID na URL não corresponde ao ID no corpo da requisição");

            try
            {
                var paciente = await _pacienteService.UpdateAsync(id, pacienteDTO);
                if (paciente == null)
                    return NotFound();

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _pacienteService.DeleteAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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