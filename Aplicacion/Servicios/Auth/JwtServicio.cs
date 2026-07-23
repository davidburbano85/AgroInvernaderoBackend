using invernaderoInteligenteBackend.Aplicacion.Interfaces.IAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.Auth
{
    public class JwtServicio: IJwtServicio
    {
        private readonly IConfiguration _config;

        public JwtServicio(IConfiguration config)
        {
            _config = config;
        }

        public string GenerarToken(Guid usuarioId, Guid invernaderoId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new Claim("InvernaderoId", invernaderoId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var token= new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
