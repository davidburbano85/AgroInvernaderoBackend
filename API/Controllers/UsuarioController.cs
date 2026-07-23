using invernaderoInteligenteBackend.Aplicacion.DTO.UsuarioDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invernaderoInteligenteBackend.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public UsuarioController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UsuarioRespuestaDto>> ObtenerUsuarioPorId(
            Guid id)
        {
            UsuarioRespuestaDto? usuario =
                await _usuarioServicio.ObtenerUsuarioPorIdAsync(id);

            return Ok(usuario);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioRespuestaDto>>> ListarUsuarios()
        {
            IEnumerable<UsuarioRespuestaDto?> usuarios =
                await _usuarioServicio.ListarUsuariosAsync();

            return Ok(usuarios);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<bool>> ActualizarUsuario(
            Guid id,
            [FromBody] ActualizarUsuarioDto dto)
        {
            bool actualizado =
                await _usuarioServicio.ActualizarUsuarioAsync(id, dto);

            return Ok(actualizado);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> EliminarUsuario(
            Guid id)
        {
            bool eliminado =
                await _usuarioServicio.EliminarUsuarioAsync(id);

            return Ok(eliminado);
        }

        [HttpPatch("{id:guid}/reactivar")]
        public async Task<ActionResult<bool>> ReactivarUsuario(
            Guid id)
        {
            bool reactivado =
                await _usuarioServicio.ReactivarUsuarioAsync(id);

            return Ok(reactivado);
        }
    }
}