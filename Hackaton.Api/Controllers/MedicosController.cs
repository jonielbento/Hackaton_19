using Hackaton.Application.DTOs;
using Hackaton.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicosController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicoDTO>>> GetAll()
        {
            var medicos = await _medicoService.GetAllAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicoDTO>> GetById(int id)
        {
            var medico = await _medicoService.GetByIdAsync(id);
            if (medico == null)
                return NotFound();

            return Ok(medico);
        }

        [HttpGet("crm/{crm}")]
        public async Task<ActionResult<MedicoDTO>> GetByCRM(string crm)
        {
            var medico = await _medicoService.GetByCRMAsync(crm);
            if (medico == null)
                return NotFound();

            return Ok(medico);
        }

        [HttpGet("especialidade/{especialidade}")]
        public async Task<ActionResult<IEnumerable<MedicoDTO>>> GetByEspecialidade(string especialidade)
        {
            var medicos = await _medicoService.GetByEspecialidadeAsync(especialidade);
            return Ok(medicos);
        }

        [HttpPost]
        public async Task<ActionResult<MedicoDTO>> Create(MedicoRegistroDTO medicoDTO)
        {
            try
            {
                var medico = await _medicoService.CreateAsync(medicoDTO);
                return CreatedAtAction(nameof(GetById), new { id = medico.Id }, medico);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MedicoDTO>> Update(int id, MedicoDTO medicoDTO)
        {
            if (id != medicoDTO.Id)
                return BadRequest("ID na URL não corresponde ao ID no corpo da requisição");

            try
            {
                var medico = await _medicoService.UpdateAsync(id, medicoDTO);
                if (medico == null)
                    return NotFound();

                return Ok(medico);
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
                var result = await _medicoService.DeleteAsync(id);
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
        public async Task<ActionResult<MedicoDTO>> Login(MedicoLoginDTO loginDTO)
        {
            var medico = await _medicoService.AuthenticateAsync(loginDTO);
            if (medico == null)
                return Unauthorized("CRM ou senha inválidos");

            return Ok(medico);
        }
    }
}