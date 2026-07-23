using invernaderoInteligenteBackend.Aplicacion.DTO.InvernaderoDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invernaderoInteligenteBackend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvernaderoController : ControllerBase
    {
        private readonly IInvernaderoServicio _invernaderoServicio;

        public InvernaderoController(IInvernaderoServicio invernaderoServicio)
        {
            _invernaderoServicio = invernaderoServicio;
        }

        [HttpPost]
        public async Task<ActionResult<InvernaderoRespuestaDto>> Crear(
            [FromBody] CrearInvernaderoDto dto)
        {
            var resultado = await _invernaderoServicio.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Id }, resultado);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvernaderoRespuestaDto>>> ObtenerMisInvernaderos()
        {
            var resultado = await _invernaderoServicio.ObtenerPorUsuarioAsync();
            return Ok(resultado);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<InvernaderoRespuestaDto>> ObtenerPorId(long id)
        {
            var resultado = await _invernaderoServicio.ObtenerPorIdAsync(id);
            return Ok(resultado);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<InvernaderoRespuestaDto>> Actualizar(
            long id,
            [FromBody] ActualizarInvernaderoDto dto)
        {
            var resultado = await _invernaderoServicio.ActualizarAsync(id, dto);
            return Ok(resultado);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Eliminar(long id)
        {
            await _invernaderoServicio.EliminarAsync(id);
            return Ok( new
            {
                mensaje = "Invernadero eliminado correctamente."
            }
                ) ;
        }


        [HttpPatch("{id:long}/reactivar")]
        public async Task<ActionResult<bool>> Reactivar(long id)
        {
            var resultado = await _invernaderoServicio.ReactivarAsync(id);

            return Ok(resultado);
        }
    }
}