using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace RadzenBook.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _http;

        //AuthenticationStateProvider cung cấp thông tin về trạng thái của người dùng(đã đăng nhập hay chưa)

        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient http)
        {
            _localStorageService = localStorageService;
            _http = http;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //lấy token từ local storage
            string authToken = await _localStorageService.GetItemAsStringAsync("authToken");

            var identity = new ClaimsIdentity();

            // unauthorized
            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    // authorized: gắn token vào request header:
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
                }
                catch
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            };

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private byte[] PasreBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }

        //parse the claims from JWT
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split(".")[1];
            var jsonBytes = PasreBase64WithoutPadding(payload);
            //Tạo key value pairs
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

            return claims;
        }
    }
}
