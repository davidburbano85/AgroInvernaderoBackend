using invernaderoInteligenteBackend.Aplicacion.DTO.TipoInstrumentoDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invernaderoInteligenteBackend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TipoInstrumentoController : ControllerBase
    {
        private readonly ITipoInstrumentoServicio _tipoInstrumentoServicio;

        public TipoInstrumentoController(ITipoInstrumentoServicio tipoInstrumentoServicio)
        {
            _tipoInstrumentoServicio = tipoInstrumentoServicio;
        }

        [HttpPost]
        public async Task<ActionResult<TipoInstrumentoRespuestaDto>> CrearTipoInstrumento(
            [FromBody] CrearTipoInstrumentoDto dto)
        {
            var tipoInstrumento =await _tipoInstrumentoServicio.CrearTipoInstrumentoAsync(dto);

            return CreatedAtAction(nameof(ObtenerTipoInstrumentoPorId),
                new { id = tipoInstrumento.Id },
                tipoInstrumento);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TipoInstrumentoRespuestaDto>> ActualizarTipoInstrumento(
            short id,
            [FromBody] ActualizarTipoInstrumentoDto dto)
        {
            var tipoInstrumento =
                await _tipoInstrumentoServicio.ActualizarTipoInstrumentoAsync(id, dto);

            return Ok(new
            {
                mensaje = "eliminado correctamente",
                tipoInstrumento            
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoInstrumentoRespuestaDto>> ObtenerTipoInstrumentoPorId(short id)
        {
            var tipoInstrumento =
                await _tipoInstrumentoServicio.ObtenerTipoInstrumentoPorIdAsync(id);

            return Ok(tipoInstrumento);
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoInstrumentoRespuestaDto>>> ObtenerTodos()
        {
            var tiposInstrumento =
                await _tipoInstrumentoServicio.ObtenerTodosTipoInstrumentosAsync();

            return Ok(tiposInstrumento);
        }
    }
}