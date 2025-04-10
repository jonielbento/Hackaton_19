using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _consultaService;

        public ConsultasController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultaDTO>>> GetAll()
        {
            var consultas = await _consultaService.GetAllAsync();
            return Ok(consultas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultaDTO>> GetById(int id)
        {
            var consulta = await _consultaService.GetByIdAsync(id);
            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        [HttpGet("medico/{medicoId}")]
        public async Task<ActionResult<IEnumerable<ConsultaDTO>>> GetByMedicoId(int medicoId)
        {
            var consultas = await _consultaService.GetByMedicoIdAsync(medicoId);
            return Ok(consultas);
        }

        [HttpGet("medico/{medicoId}/data/{data}")]
        public async Task<ActionResult<IEnumerable<ConsultaDTO>>> GetByMedicoIdEData(int medicoId, DateTime data)
        {
            var consultas = await _consultaService.GetByMedicoIdAsync(medicoId);
            var consultasFiltradas = consultas.Where(c => c.DataHora.Date == data.Date);
            return Ok(consultasFiltradas);
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<ConsultaDTO>>> GetByPacienteId(int pacienteId)
        {
            try
            {
                Console.WriteLine($"Recebendo requisição para buscar consultas do paciente {pacienteId}");
                var consultas = await _consultaService.GetByPacienteIdAsync(pacienteId);
                Console.WriteLine($"Consultas encontradas: {consultas?.Count() ?? 0}");
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar consultas do paciente {pacienteId}: {ex.Message}");
                return StatusCode(500, $"Erro interno ao buscar consultas: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ConsultaDTO>> Create(ConsultaRegistroDTO consultaDTO)
        {
            try
            {
                var consulta = await _consultaService.CreateAsync(consultaDTO);
                return CreatedAtAction(nameof(GetById), new { id = consulta.Id }, consulta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/aceitar")]
        public async Task<ActionResult<ConsultaDTO>> AceitarConsulta(int id)
        {
            var consulta = await _consultaService.AceitarConsultaAsync(id);
            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<ActionResult<ConsultaDTO>> CancelarConsulta(int id, [FromBody] string justificativa)
        {
            var consulta = await _consultaService.CancelarConsultaAsync(id, justificativa);
            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        [HttpPut("{id}/recusar")]
        public async Task<ActionResult<ConsultaDTO>> RecusarConsulta(int id, [FromBody] string justificativa)
        {
            var consulta = await _consultaService.RejeitarConsultaAsync(id, justificativa);
            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        [HttpPut("{id}/concluir")]
        public async Task<ActionResult<ConsultaDTO>> ConcluirConsulta(int id)
        {
            var consulta = await _consultaService.ConcluirConsultaAsync(id);
            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _consultaService.DeleteAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}