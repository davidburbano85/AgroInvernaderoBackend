using invernaderoInteligenteBackend.Aplicacion.DTO.Medicion;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using invernaderoInteligenteBackend.Dominio.Entidades.ControladorIot;
using invernaderoInteligenteBackend.Dominio.Entidades.Mediciones;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.ServiciosDto
{
    public class MedicionServicio : IMedicionServicio
    {
        private readonly IMedicionRepositorio _medicionRepositorio;
        private readonly IInstrumentoRepositorio _instrumentoRepositorio;
        private readonly IUsuarioContext _usuarioContext;
        private readonly IControladorContext _controladorContext;
        private readonly IControladorIotRepositorio _controladorIotRepositorio;

        public MedicionServicio(
            IMedicionRepositorio medicionRepositorio,
            IInstrumentoRepositorio instrumentoRepositorio,
            IUsuarioContext usuarioContext,
            IControladorContext controladorContext,
            IControladorIotRepositorio controladorIotRepositorio)
        {
            _medicionRepositorio = medicionRepositorio;
            _instrumentoRepositorio = instrumentoRepositorio;
            _usuarioContext = usuarioContext;
            _controladorContext = controladorContext;
            _controladorIotRepositorio = controladorIotRepositorio;
        }

        public async Task<long> CrearMedicionAsync(CrearMedicionDto dto)
        {
            bool existeInstrumento =
                await _instrumentoRepositorio.ExisteInstrumentoAsync(dto.InstrumentoId);

            if (!existeInstrumento)
                throw new KeyNotFoundException("El instrumento no existe.");

            var medicion = new Medicion
            {
                InstrumentoId = dto.InstrumentoId,
                CoordenadaX = dto.CoordenadaX,
                CoordenadaY = dto.CoordenadaY,
                Cantidad = dto.Cantidad,
                FechaHora = DateTime.UtcNow,
                Estado = dto.Estado,
                Activo = true
            };

            long id = await _medicionRepositorio.CrearMedicionAsync(medicion);

            return id;
        }

        public async Task<long> CrearMedicionIotAsync(CrearMedicionDto dto)
        {
            Guid token =
                _controladorContext.ObtenerTokenControlador();


            ControladorIot controlador =
                await _controladorIotRepositorio
                    .ObtenerControladorPorToken(token);


            bool instrumentoValido =
                await _instrumentoRepositorio
                    .ExisteInstrumentoPorControladorAsync(
                        dto.InstrumentoId,
                        controlador.Id);


            if (!instrumentoValido)
                throw new UnauthorizedAccessException(
                    "El instrumento no pertenece al controlador IoT.");


            var medicion = new Medicion
            {
                InstrumentoId = dto.InstrumentoId,
                CoordenadaX = dto.CoordenadaX,
                CoordenadaY = dto.CoordenadaY,
                Cantidad = dto.Cantidad,
                FechaHora = DateTime.UtcNow,
                Estado = dto.Estado,
                Activo = true
            };


            return await _medicionRepositorio
                .CrearMedicionAsync(medicion);
        }


        public async Task<MedicionRespuestaDto> ActualizarMedicionAsync(
      long id,
      ActualizarMedicionDto dto)
        {
            bool existe =
                await _medicionRepositorio.ExisteMedicionAsync(id);

            if (!existe)
                throw new KeyNotFoundException("La medición no existe.");

            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            Medicion? medicionActual =
                await _medicionRepositorio.ObtenerMedicionPorIdAsync(id, usuarioId);

            if (medicionActual is null)
                throw new KeyNotFoundException("La medición no existe.");

            var medicion = new Medicion
            {
                Id = id,
                Estado = dto.Estado,
                Activo = dto.Activo,
                FechaHora = dto.FechaHora
            };

            await _medicionRepositorio.ActualizarMedicionAsync(medicion);

            Medicion? actualizada =
                await _medicionRepositorio.ObtenerMedicionPorIdAsync(id, usuarioId);

            if (actualizada is null)
                throw new KeyNotFoundException("La medición no existe.");

            return new MedicionRespuestaDto
            {
                Id = actualizada.Id,
                InstrumentoId = actualizada.InstrumentoId,
                CoordenadaX = actualizada.CoordenadaX,
                CoordenadaY = actualizada.CoordenadaY,
                Cantidad = actualizada.Cantidad,
                FechaHora = actualizada.FechaHora,
                Estado = actualizada.Estado,
                Activo = actualizada.Activo
            };
        }
        public async Task<bool> EliminarMedicionLogicoAsync(long id)
        {
            bool existe =
                await _medicionRepositorio.ExisteMedicionAsync(id);

            if (!existe)
                throw new KeyNotFoundException("La medición no existe.");

            return await _medicionRepositorio.EliminarMedicionLogicoAsync(id);
        }

        public async Task<MedicionRespuestaDto?> ObtenerMedicionPorIdAsync(long id)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            Medicion? medicion =
                await _medicionRepositorio.ObtenerMedicionPorIdAsync(id, usuarioId);

            if (medicion is null)
                throw new KeyNotFoundException("La medición no existe.");

            return new MedicionRespuestaDto
            {
                Id = medicion.Id,
                InstrumentoId = medicion.InstrumentoId,
                CoordenadaX = medicion.CoordenadaX,
                CoordenadaY = medicion.CoordenadaY,
                Cantidad = medicion.Cantidad,
                FechaHora = medicion.FechaHora,
                Estado = medicion.Estado,
                Activo = medicion.Activo
            };
        }

        public async Task<IEnumerable<MedicionRespuestaDto>> ObtenerMedicionPorInstrumentoAsync(
            long instrumentoId)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            IEnumerable<Medicion> mediciones =
                await _medicionRepositorio.ObtenerMedicionPorInstrumentoAsync(
                    instrumentoId,
                    usuarioId);

            return mediciones.Select(medicion => new MedicionRespuestaDto
            {
                Id = medicion.Id,
                InstrumentoId = medicion.InstrumentoId,
                CoordenadaX = medicion.CoordenadaX,
                CoordenadaY = medicion.CoordenadaY,
                Cantidad = medicion.Cantidad,
                FechaHora = medicion.FechaHora,
                Estado = medicion.Estado,
                Activo = medicion.Activo
            });
        }

        public async Task<IEnumerable<MedicionRespuestaDto>> ObtenerMedicionPorRangoFechasAsync(
            long instrumentoId,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            IEnumerable<Medicion> mediciones =
                await _medicionRepositorio.ObtenerMedicionPorRangoFechasAsync(
                    instrumentoId,
                    fechaInicio,
                    fechaFin,
                    usuarioId);

            return mediciones.Select(medicion => new MedicionRespuestaDto
            {
                Id = medicion.Id,
                InstrumentoId = medicion.InstrumentoId,
                CoordenadaX = medicion.CoordenadaX,
                CoordenadaY = medicion.CoordenadaY,
                Cantidad = medicion.Cantidad,
                FechaHora = medicion.FechaHora,
                Estado = medicion.Estado,
                Activo = medicion.Activo
            });
        }
        public async Task<IEnumerable<MedicionRespuestaDto>> ObtenerTodasLasMedicionesAsync()
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            IEnumerable<Medicion> mediciones =
                await _medicionRepositorio.ObtenerTodasLasMedicionesAsync(usuarioId);

            return mediciones.Select(medicion => new MedicionRespuestaDto
            {
                Id = medicion.Id,
                InstrumentoId = medicion.InstrumentoId,
                CoordenadaX = medicion.CoordenadaX,
                CoordenadaY = medicion.CoordenadaY,
                Cantidad = medicion.Cantidad,
                FechaHora = medicion.FechaHora,
                Estado = medicion.Estado,
                Activo = medicion.Activo
            });
        }

    }
}