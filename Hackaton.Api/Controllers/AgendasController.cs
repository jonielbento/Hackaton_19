using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendasController : ControllerBase
    {
        private readonly IAgendaService _agendaService;

        public AgendasController(IAgendaService agendaService)
        {
            _agendaService = agendaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgendaDTO>>> GetAll()
        {
            var agendas = await _agendaService.GetAllAsync();
            return Ok(agendas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgendaDTO>> GetById(int id)
        {
            var agenda = await _agendaService.GetByIdAsync(id);
            if (agenda == null)
                return NotFound();

            return Ok(agenda);
        }

        [HttpGet("medico/{medicoId}")]
        public async Task<ActionResult<IEnumerable<AgendaDTO>>> GetByMedicoId(int medicoId)
        {
            var agendas = await _agendaService.GetByMedicoIdAsync(medicoId);
            return Ok(agendas);
        }

        [HttpGet("disponiveis/medico/{medicoId}")]
        public async Task<ActionResult<IEnumerable<AgendaDTO>>> GetDisponiveisByMedicoId(int medicoId)
        {
            var agendas = await _agendaService.GetDisponiveisByMedicoIdAsync(medicoId);
            return Ok(agendas);
        }

        [HttpGet("disponiveis/{medicoId}/{data}")]
        public async Task<ActionResult<IEnumerable<AgendaDTO>>> GetDisponiveisByMedicoIdEData(int medicoId, DateTime data)
        {
            try
            {
                // Primeiro buscar todas as agendas para a data selecionada, independente do status
                var agendas = await _agendaService.GetByMedicoIdAsync(medicoId);
                var agendasDoDia = agendas.Where(a => a.DataHoraInicio.Date == data.Date).ToList();
                
                // Opcional: Log para depuração
                Console.WriteLine($"Encontradas {agendasDoDia.Count} agendas para o médico {medicoId} na data {data:yyyy-MM-dd}");
                foreach (var agenda in agendasDoDia)
                {
                    Console.WriteLine($"Agenda {agenda.Id}: {agenda.DataHoraInicio} - {agenda.DataHoraFim}, Disponível: {agenda.Disponivel}");
                }
                
                return Ok(agendasDoDia);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AgendaDTO>> Create(AgendaRegistroDTO agendaDTO)
        {
            try
            {
                var agenda = await _agendaService.CreateAsync(agendaDTO);
                return CreatedAtAction(nameof(GetById), new { id = agenda.Id }, agenda);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AgendaDTO>> Update(int id, AgendaDTO agendaDTO)
        {
            if (id != agendaDTO.Id)
                return BadRequest("ID na URL não corresponde ao ID no corpo da requisição");

            try
            {
                var agenda = await _agendaService.UpdateAsync(id, agendaDTO);
                if (agenda == null)
                    return NotFound();

                return Ok(agenda);
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
                var result = await _agendaService.DeleteAsync(id);
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