using invernaderoInteligenteBackend.Aplicacion.DTO.InstrumentoDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invernaderoInteligenteBackend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InstrumentoController : ControllerBase
    {
        private readonly IInstrumentoServicio _instrumentoServicio;

        public InstrumentoController(
            IInstrumentoServicio instrumentoServicio)
        {
            _instrumentoServicio = instrumentoServicio;
        }

        [HttpPost]
        public async Task<ActionResult<InstrumentoRespuestaDto>> Crear(
            [FromBody] CrearInstrumentoDto dto)
        {
            var instrumento =
                await _instrumentoServicio.CrearInstrumentoAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = instrumento.Id },
                instrumento);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<InstrumentoRespuestaDto>> Actualizar(
            long id,
            [FromBody] ActualizarInstrumentoDto dto)
        {
            var instrumento =
                await _instrumentoServicio.ActualizarInstrumentoAsync(id, dto);

            return Ok(instrumento);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Eliminar(long id)
        {
            bool eliiminado =await _instrumentoServicio.EliminarInstrumentoAsync(id);

            return Ok(new
                {
                   mensaje=  "eliminado correctamente",
                eliiminado
                    }
                );
        }

        [HttpPatch("{id:long}/reactivar")]
        public async Task<IActionResult> Reactivar(long id)
        {
            await _instrumentoServicio.ReactivarInstrumentoAsync(id);

            return NoContent();
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<InstrumentoRespuestaDto>> ObtenerPorId(long id)
        {
            var instrumento =
                await _instrumentoServicio.ObtenerInstrumentoPorIdAsync(id);

            return Ok(instrumento);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstrumentoRespuestaDto>>> ObtenerPorControlador()
        {
            var instrumentos =
                await _instrumentoServicio.ObtenerInstrumentoPorControladorAsync();

            return Ok(instrumentos);
        }
    }
}