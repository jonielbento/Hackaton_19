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
            try
            {
                var medicos = await _medicoService.GetAllAsync();
                return Ok(medicos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar médicos: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao buscar médicos: " + ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicoDTO>> GetById(int id)
        {
            try
            {
                var medico = await _medicoService.GetByIdAsync(id);
                if (medico == null)
                    return NotFound(new { message = $"Médico com ID {id} não encontrado" });

                return Ok(medico);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar médico por ID: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao buscar médico: " + ex.Message });
            }
        }

        [HttpGet("crm/{crm}")]
        public async Task<ActionResult<MedicoDTO>> GetByCRM(string crm)
        {
            try
            {
                var medico = await _medicoService.GetByCRMAsync(crm);
                if (medico == null)
                    return NotFound(new { message = $"Médico com CRM {crm} não encontrado" });

                return Ok(medico);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar médico por CRM: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao buscar médico: " + ex.Message });
            }
        }

        [HttpGet("especialidade/{especialidade}")]
        public async Task<ActionResult<IEnumerable<MedicoDTO>>> GetByEspecialidade(string especialidade)
        {
            try
            {
                var medicos = await _medicoService.GetByEspecialidadeAsync(especialidade);
                return Ok(medicos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar médicos por especialidade: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao buscar médicos: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<MedicoDTO>> Create(MedicoRegistroDTO medicoDTO)
        {
            try
            {
                Console.WriteLine($"Iniciando cadastro de médico: {medicoDTO.Nome}, CRM: {medicoDTO.CRM}");
                var medico = await _medicoService.CreateAsync(medicoDTO);
                Console.WriteLine($"Médico cadastrado com sucesso! ID: {medico.Id}");
                return CreatedAtAction(nameof(GetById), new { id = medico.Id }, medico);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar médico: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MedicoDTO>> Update(int id, MedicoDTO medicoDTO)
        {
            if (id != medicoDTO.Id)
                return BadRequest(new { message = "ID na URL não corresponde ao ID no corpo da requisição" });

            try
            {
                var medico = await _medicoService.UpdateAsync(id, medicoDTO);
                if (medico == null)
                    return NotFound(new { message = $"Médico com ID {id} não encontrado" });

                return Ok(medico);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar médico: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _medicoService.DeleteAsync(id);
                if (!result)
                    return NotFound(new { message = $"Médico com ID {id} não encontrado" });

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir médico: {ex.Message}");
                return BadRequest(new { message = ex.Message });
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