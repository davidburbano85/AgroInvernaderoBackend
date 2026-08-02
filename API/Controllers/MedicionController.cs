using invernaderoInteligenteBackend.Aplicacion.DTO.Medicion;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invernaderoInteligenteBackend.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MedicionController : ControllerBase
    {
        private readonly IMedicionServicio _medicionServicio;

        public MedicionController(IMedicionServicio medicionServicio)
        {
            _medicionServicio = medicionServicio;
        }

        [HttpPost]
        public async Task<ActionResult<long>> CrearMedicion(
            [FromBody] CrearMedicionDto dto)
        {
            long id = await _medicionServicio.CrearMedicionAsync(dto);

            return Ok(id);
        }

        [AllowAnonymous]
        [HttpPost("iot")]
        public async Task<IActionResult> CrearMedicionIot(
           [FromBody]
        CrearMedicionDto dto)
        {
            long id =
                await _medicionServicio.CrearMedicionIotAsync(dto);


            return Ok(new
            {
                id,
                mensaje = "Medición registrada correctamente."
            });
        }


        [HttpPut("{id:long}")]
        public async Task<ActionResult<MedicionRespuestaDto>> ActualizarMedicion(
            long id,
            [FromBody] ActualizarMedicionDto dto)
        {
            MedicionRespuestaDto medicion =
                await _medicionServicio.ActualizarMedicionAsync(id, dto);

            return Ok(medicion);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<bool>> EliminarMedicion(
            long id)
        {
            bool eliminado =
                await _medicionServicio.EliminarMedicionLogicoAsync(id);

            return Ok(eliminado);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MedicionRespuestaDto>> ObtenerMedicionPorId(
            long id)
        {
            MedicionRespuestaDto? medicion =
                await _medicionServicio.ObtenerMedicionPorIdAsync(id);

            return Ok(medicion);
        }

        [HttpGet("instrumento/{instrumentoId:long}")]
        public async Task<ActionResult<IEnumerable<MedicionRespuestaDto>>> ObtenerMedicionesPorInstrumento(
            long instrumentoId)
        {
            IEnumerable<MedicionRespuestaDto> mediciones =
                await _medicionServicio.ObtenerMedicionPorInstrumentoAsync(instrumentoId);

            return Ok(mediciones);
        }

        [HttpGet("instrumento/{instrumentoId:long}/rango-fechas")]
        public async Task<ActionResult<IEnumerable<MedicionRespuestaDto>>> ObtenerMedicionesPorRangoFechas(
            long instrumentoId,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            IEnumerable<MedicionRespuestaDto> mediciones =
                await _medicionServicio.ObtenerMedicionPorRangoFechasAsync(
                    instrumentoId,
                    fechaInicio,
                    fechaFin);

            return Ok(mediciones);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicionRespuestaDto>>> ObtenerTodasLasMediciones()
        {
            IEnumerable<MedicionRespuestaDto> mediciones =
                await _medicionServicio.ObtenerTodasLasMedicionesAsync();

            return Ok(mediciones);
        }
    }
}