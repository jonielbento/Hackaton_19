using System.Net.Http.Json;
using Hackaton.Web.Models;

namespace Hackaton.Web.Services
{
    public class ConsultaService
    {
        private readonly HttpClient _httpClient;

        public ConsultaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AgendarAsync(ConsultaRegistroModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/consultas", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelarAsync(int id, string justificativa)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/consultas/{id}/cancelar", justificativa);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AceitarAsync(int id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/consultas/{id}/aceitar", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RejeitarAsync(int id, string justificativa)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/consultas/{id}/rejeitar", justificativa);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ConcluirAsync(int id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/consultas/{id}/concluir", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
} 