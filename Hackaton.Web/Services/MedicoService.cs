using Hackaton.Web.Models;
using System.Net.Http.Json;

namespace Hackaton.Web.Services
{
    public class MedicoService
    {
        private readonly HttpClient _httpClient;

        public MedicoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterAsync(MedicoRegistroModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/medicos", new
                {
                    model.Nome,
                    model.CRM,
                    model.Especialidade,
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