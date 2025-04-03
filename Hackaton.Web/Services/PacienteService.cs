using Hackaton.Web.Models;
using System.Net.Http.Json;

namespace Hackaton.Web.Services
{
    public class PacienteService
    {
        private readonly HttpClient _httpClient;

        public PacienteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterAsync(PacienteRegistroModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/pacientes", new
                {
                    model.Nome,
                    model.CPF,
                    model.Email,
                    model.Telefone,
                    model.Senha
                });

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ... existing code ...
    }
} 