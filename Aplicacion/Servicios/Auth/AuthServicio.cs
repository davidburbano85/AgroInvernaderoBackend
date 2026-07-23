using invernaderoInteligenteBackend.Aplicacion.DTO.AuthDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IAuth;
using invernaderoInteligenteBackend.Infraestructura.Auth;
using System.Text;
using System.Text.Json;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.Auth
{
    public class AuthServicio: IAuthServicio
    {
     
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AuthServicio(HttpClient httpClient, 
                            IConfiguration config
                          
                           )
        {
            _httpClient = httpClient;
            _config = config;

        }

        public async Task<AuthRespuestaDto> LoginAsync(string email, string password)
        {
            var urlAppsettings = _config.GetSection("supabase") ["ProjectUrl"];
            var url = $"{urlAppsettings}/auth/v1/token?grant_type=password";
            var anonKey = _config.GetSection("Supabase")["AnonKey"];

            var request= new
            {   
                email,
                password
            };
            
            var requestJson= JsonSerializer.Serialize(request);// Convertir el objeto a JSON
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("apikey", anonKey);
            httpRequest.Content = new StringContent(
                requestJson, 
                Encoding.UTF8, 
                "application/json");
            var response = await _httpClient.SendAsync(httpRequest);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);
            var json= JsonSerializer.Deserialize<JsonElement>(content);
            var accessToken = json.GetProperty("access_token").GetString();
            var userId = json.GetProperty("user")
                .GetProperty("id")
                .GetString();
            return new AuthRespuestaDto
            {
                AccessToken = accessToken,
                UserId = Guid.Parse(userId)
            };

        }

        public async Task<AuthRespuestaDto> SignUpAsync(string email, string password)
        {
            var urlAppsettings = _config.GetSection("supabase")["ProjectUrl"];
            var url = $"{urlAppsettings}/auth/v1/signup";
            var anonKey = _config.GetSection("Supabase")["AnonKey"];


            if (string.IsNullOrWhiteSpace(anonKey))
                throw new Exception("La clave anónima de Supabase no está configurada.");
            var request = new
            {
                email,
                password
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("apikey", anonKey);
            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.SendAsync(httpRequest);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);
            var json = JsonSerializer.Deserialize<JsonElement>(content);
            var accessToken = json.GetProperty("access_token").GetString();
            var refreshToken = json.GetProperty("refresh_token").GetString();
            var userId = json.GetProperty("user")
                .GetProperty("id")
                .GetString();

            return new AuthRespuestaDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = Guid.Parse(userId)
            };
        }

        public async Task<AuthRespuestaDto> RefreshTokenAsync(string refreshToken)
        {
            var urlAppsettings = _config.GetSection("supabase")["ProjectUrl"];
            var url = $"{urlAppsettings}/auth/v1/token?grant_type=refresh_token";
            var anonKey = _config.GetSection("Supabase")["AnonKey"];

            var request = new
            {
                refresh_token = refreshToken
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("apikey", anonKey);
            httpRequest.Headers.Add("Authorization", $"Bearer {anonKey}");
            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);

            var json = JsonSerializer.Deserialize<JsonElement>(content);

            return new AuthRespuestaDto
            {
                AccessToken = json.GetProperty("access_token").GetString(),
                RefreshToken = json.GetProperty("refresh_token").GetString(),
            };


        }





    }
}
