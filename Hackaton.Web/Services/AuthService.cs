using Hackaton.Web.Models;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace Hackaton.Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private UserSession _currentUser = new UserSession();

        public UserSession CurrentUser => _currentUser;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            var savedSession = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userSession");
            if (!string.IsNullOrEmpty(savedSession))
            {
                try
                {
                    var userSession = JsonSerializer.Deserialize<UserSession>(savedSession);
                    if (userSession != null && userSession.IsAuthenticated)
                    {
                        _currentUser = userSession;
                    }
                    else
                    {
                        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userSession");
                    }
                }
                catch
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userSession");
                }
            }
        }

        public async Task<AuthResponseModel?> LoginMedicoAsync(MedicoLoginModel model)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("console.log", "Tentando fazer login com:", model);
                await _jsRuntime.InvokeVoidAsync("console.log", "URL base:", _httpClient.BaseAddress);
                
                var response = await _httpClient.PostAsJsonAsync("api/Auth/medico/login", model);
                
                await _jsRuntime.InvokeVoidAsync("console.log", "Status code:", response.StatusCode);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponseModel>();
                    await _jsRuntime.InvokeVoidAsync("console.log", "Resposta:", result);
                    return result;
                }
                
                var error = await response.Content.ReadAsStringAsync();
                await _jsRuntime.InvokeVoidAsync("console.log", "Erro:", error);
                return null;
            }
            catch (Exception ex)
            {
                await _jsRuntime.InvokeVoidAsync("console.log", "Exceção:", ex.Message);
                return null;
            }
        }

        public async Task<AuthResponseModel?> LoginPacienteAsync(PacienteLoginModel model)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("console.log", "Tentando fazer login com:", model);
                await _jsRuntime.InvokeVoidAsync("console.log", "URL base:", _httpClient.BaseAddress);
                
                var response = await _httpClient.PostAsJsonAsync("api/Auth/paciente/login", model);
                
                await _jsRuntime.InvokeVoidAsync("console.log", "Status code:", response.StatusCode);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponseModel>();
                    await _jsRuntime.InvokeVoidAsync("console.log", "Resposta:", result);
                    return result;
                }
                
                var error = await response.Content.ReadAsStringAsync();
                await _jsRuntime.InvokeVoidAsync("console.log", "Erro:", error);
                return null;
            }
            catch (Exception ex)
            {
                await _jsRuntime.InvokeVoidAsync("console.log", "Exceção:", ex.Message);
                return null;
            }
        }

        public async Task SetUserSessionAsync(AuthResponseModel authResponse)
        {
            _currentUser = new UserSession
            {
                UserType = authResponse.UserType,
                UserId = authResponse.UserId,
                Nome = authResponse.Nome
            };

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userSession", JsonSerializer.Serialize(_currentUser));
        }

        public async Task LogoutAsync()
        {
            _currentUser = new UserSession();
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userSession");
        }

        public bool IsUserAuthenticated()
        {
            return _currentUser.IsAuthenticated;
        }

        public bool IsMedico()
        {
            return _currentUser.IsAuthenticated && _currentUser.UserType == "Medico";
        }

        public bool IsPaciente()
        {
            return _currentUser.IsAuthenticated && _currentUser.UserType == "Paciente";
        }
    }
}