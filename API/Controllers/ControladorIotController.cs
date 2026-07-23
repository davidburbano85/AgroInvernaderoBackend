using invernaderoInteligenteBackend.Aplicacion.DTO.ControladorDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invernaderoInteligenteBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ControladorIotController : ControllerBase
    {
        private readonly IControladorIotServicio _controladorIotServicio;

        public ControladorIotController(
            IControladorIotServicio controladorIotServicio)
        {
            _controladorIotServicio = controladorIotServicio;
        }


        [HttpPost]
        public async Task<IActionResult> Crear(
     [FromBody] CrearControladorIotDto dto)
        {
            var respuesta =
                await _controladorIotServicio.CrearControladorIotAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorInvernadero),
                new { invernaderoId = respuesta.InvernaderoId },
                respuesta);
        }


        [HttpPut("{id:long}")]
        public async Task<IActionResult> Actualizar(
            long id,
            [FromBody] ActualizarControladorIotDto dto)
        {
            var respuesta =
                await _controladorIotServicio.ActualizarControladorIotAsync(id, dto);

            return Ok(respuesta);
        }


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Eliminar(long id)
        {
            bool eliminado =
                await _controladorIotServicio.EliminarControladorIotAsync(id);

            return Ok(new
            {
                mensaje = "Controlador IoT eliminado correctamente.",
                eliminado
            });
        }


       


        [HttpGet("invernadero")]
        public async Task<IActionResult> ObtenerPorInvernadero()
        {
            var respuesta =
                await _controladorIotServicio
                    .ObtenerControladorIotPorInvernaderoAsync();


            if (respuesta is null)
                return NotFound(new
                {
                    mensaje = "El usuario no tiene un controlador IoT registrado."
                });


            return Ok(respuesta);
        }


        [HttpPatch("{id:long}/reactivar")]
        public async Task<IActionResult> Reactivar(long id)
        {
            bool reactivado =
                await _controladorIotServicio.ReactivarControladorIotAsync(id);

            return Ok(new
            {
                mensaje = "Controlador IoT reactivado correctamente.",
                reactivado
            });
        }
    }
}