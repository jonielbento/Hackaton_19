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
                var response = await _httpClient.PostAsJsonAsync("api/Auth/medico/login", model);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponseModel>();
                    if (result != null)
                    {
                        await SetUserSessionAsync(result);
                    }
                    return result;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<AuthResponseModel?> LoginPacienteAsync(PacienteLoginModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/paciente/login", model);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponseModel>();
                    if (result != null)
                    {
                        await SetUserSessionAsync(result);
                    }
                    return result;
                }
                return null;
            }
            catch
            {
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