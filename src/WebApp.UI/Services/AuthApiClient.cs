using System.Net.Http.Json;

namespace WebApp.UI.Services
{
    public interface IAuthApiClient
    {
        Task<LoginResponse> LoginAsync(string userName, string password);
    }

    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _http;

        public AuthApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<LoginResponse> LoginAsync(string userName, string password)
        {
            var payload = new { userName, password };
            var response = await _http.PostAsJsonAsync("/api/auth/login", payload);

            if (!response.IsSuccessStatusCode)
            {
                return new LoginResponse(false, "Credenciales inválidas");
            }

            var data = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return data ?? new LoginResponse(false, "Error inesperado");
        }
    }

    public record LoginResponse(bool Success, string Message);
}
