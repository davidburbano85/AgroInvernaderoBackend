using invernaderoInteligenteBackend.Aplicacion.DTO.AuthDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IAuth;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork;
using invernaderoInteligenteBackend.Dominio.Entidades.Usuario;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace invernaderoInteligenteBackend.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthServicio _authServicio;
        private readonly IUsuarioRepositorioDapper _usuarioRepositorioDapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthServicio authServicio,
                             IUsuarioRepositorioDapper usuarioRepositorioDapper,
                                IUnitOfWork unitOfWork
                                       )
        {
            _authServicio = authServicio;
            _usuarioRepositorioDapper = usuarioRepositorioDapper;
            _unitOfWork = unitOfWork;

        }
        // =========================
        // LOGIN
        // =========================

        [HttpPost("/api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                Console.WriteLine("========== LOGIN ==========");
                Console.WriteLine($"Email recibido: {dto.Email}");

                var result = await _authServicio.LoginAsync(dto.Email, dto.Password);

                Console.WriteLine("========== RESPUESTA LOGIN ==========");


                // 🔥 LOG DEL USER ID
                if (result.UserId != Guid.Empty)
                {
                    Console.WriteLine($"AUTH USER ID: {result.UserId}");
                }
                else
                {
                    Console.WriteLine("NO SE RECIBIÓ USER ID EN EL RESULTADO");
                }

                Console.WriteLine("=====================================");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR EN LOGIN:");
                Console.WriteLine(ex.Message);

                return StatusCode(500, $"Error al iniciar sesión: {ex.Message}");
            }
        }



        //******************************
        // CREARUSUARIO
        //******************

        [HttpPost("signup")]
        public async Task<IActionResult> CrearUsuario([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _authServicio.SignUpAsync(
                    dto.Email,
                    dto.Password);

                if (result.UserId == Guid.Empty)
                {
                    Console.WriteLine("Error: userId vacío");
                    return BadRequest("No se pudo obtener el userID.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }













        [HttpPost("refresh--token")]

        public async Task<IActionResult> RefreshTokenAsync([FromBody] AuthRespuestaDto dto)
        {
            try
            {
                var result = await _authServicio.RefreshTokenAsync(dto.RefreshToken);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}




