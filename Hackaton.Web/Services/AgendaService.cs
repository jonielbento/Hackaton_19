using System.Net.Http.Json;
using Hackaton.Web.Models;

namespace Hackaton.Web.Services
{
    public class AgendaService
    {
        private readonly HttpClient _httpClient;

        public AgendaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CriarAsync(AgendaRegistroModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/agendas", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AtualizarAsync(int id, AgendaModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/agendas/{id}", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/agendas/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
} 