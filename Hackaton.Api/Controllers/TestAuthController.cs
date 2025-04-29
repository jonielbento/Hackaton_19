using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : ControllerBase
    {
        [HttpGet("publico")]
        public IActionResult GetPublico()
        {
            return Ok(new { mensagem = "Endpoint público acessível a todos" });
        }

        [HttpGet("autenticado")]
        [Authorize]
        public IActionResult GetAutenticado()
        {
            return Ok(new { mensagem = "Endpoint acessível apenas a usuários autenticados" });
        }

        [HttpGet("medico")]
        [Authorize(Roles = "Medico")]
        public IActionResult GetMedico()
        {
            return Ok(new { mensagem = "Endpoint acessível apenas a médicos" });
        }

        [HttpGet("paciente")]
        [Authorize(Roles = "Paciente")]
        public IActionResult GetPaciente()
        {
            return Ok(new { mensagem = "Endpoint acessível apenas a pacientes" });
        }
    }
} 