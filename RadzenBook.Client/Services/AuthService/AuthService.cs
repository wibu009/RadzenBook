using System.Net.Http.Json;

namespace RadzenBook.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(UserChangePasswordDTO req)
        {
            var result = await _http.PostAsJsonAsync("api/auth/change-password", req.NewPassword);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<string>> Login(UserLoginDTO req)
        {
            var result = await _http.PostAsJsonAsync("api/auth/login", req);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterDTO req)
        {
            var result = await _http.PostAsJsonAsync("api/auth/register", req);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
