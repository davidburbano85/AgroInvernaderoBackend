using invernaderoInteligenteBackend.Aplicacion.DTO.InstrumentoDto;
using invernaderoInteligenteBackend.Aplicacion.DTO.TipoInstrumentoDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using invernaderoInteligenteBackend.Dominio.Entidades.ControladorIot;
using invernaderoInteligenteBackend.Dominio.Entidades.Instrumentos;
using invernaderoInteligenteBackend.Dominio.Entidades.Invernadero;
using invernaderoInteligenteBackend.Infraestructura.Mapper.MapperDominio;
using invernaderoInteligenteBackend.Infraestructura.Mapper.MapperDto;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.ServiciosDto
{
    public class InstrumentoServicio : IInstrumentoServicio
    {
        private readonly IInstrumentoRepositorio _instrumentoRepositorio;
        private readonly IControladorIotRepositorio _controladorRepositorio;
        private readonly ITipoInstrumentoRepositorio _tipoInstrumentoRepositorio;
        private readonly IUsuarioContext _usuarioContext;
        private readonly IInvernaderoRepositorio _invernaderoRepositorio;

        public InstrumentoServicio(
            IInstrumentoRepositorio instrumentoRepositorio,
            IControladorIotRepositorio controladorRepositorio,
            ITipoInstrumentoRepositorio tipoInstrumentoRepositorio,
            IUsuarioContext usuarioContext,
            IInvernaderoRepositorio invernaderoRepositorio)
        {
            _instrumentoRepositorio = instrumentoRepositorio;
            _controladorRepositorio = controladorRepositorio;
            _tipoInstrumentoRepositorio = tipoInstrumentoRepositorio;
            _usuarioContext = usuarioContext;
            _invernaderoRepositorio = invernaderoRepositorio;
        }

        public async Task<InstrumentoRespuestaDto> CrearInstrumentoAsync(CrearInstrumentoDto dto, Guid token)
        {
      

            ControladorIot? controlador=await _controladorRepositorio
                .ObtenerControladorPorToken(token);
            if (controlador is null)
                throw new InvalidOperationException("El controlador IoT no existe.");

            bool existeTipo =
                await _tipoInstrumentoRepositorio.ExisteTipoInstrumentoAsync(dto.TipoInstrumentoId);

            if (!existeTipo)
                throw new KeyNotFoundException("El tipo de instrumento no existe.");

          

           

            var instrumento = new Instrumento
            {
                ControladorId = controlador.Id,
                TipoInstrumentoId = dto.TipoInstrumentoId,
                Referencia = dto.Referencia,
                Estado = EstadoInstrumentoMapper.ToDominio(dto.Estado),
                Activo = true
            };

            long id =
                await _instrumentoRepositorio.CrearInstrumentoAsync(instrumento);

            instrumento.Id = id;

            return new InstrumentoRespuestaDto
            {
                Id = instrumento.Id,
                ControladorId = instrumento.ControladorId,
                TipoInstrumentoId = instrumento.TipoInstrumentoId,
                Referencia = instrumento.Referencia,
                Estado = EstadoInstrumentoMapperDto.ToDto(instrumento.Estado),
                Activo = instrumento.Activo
            };
        }

        public async Task<InstrumentoRespuestaDto> ActualizarInstrumentoAsync(
            long id,
            ActualizarInstrumentoDto dto)
        {
            bool existe =
                await _instrumentoRepositorio.ExisteInstrumentoAsync(id);

            if (!existe)
                throw new KeyNotFoundException("El instrumento no existe.");

            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            Instrumento? instrumentoActual =
                await _instrumentoRepositorio.ObtenerInstrumentoPorIdAsync(id, usuarioId);

            if (instrumentoActual is null)
                throw new KeyNotFoundException("El instrumento no existe.");

            bool existeTipo =
                await _tipoInstrumentoRepositorio.ExisteTipoInstrumentoAsync(dto.TipoInstrumentoId);

            if (!existeTipo)
                throw new KeyNotFoundException("El tipo de instrumento no existe.");

            bool referenciaDuplicada =
                await _instrumentoRepositorio.ExisteReferenciaActualizarInstrumentoAsync(
                    id,
                    instrumentoActual.ControladorId,
                    dto.Referencia);

            if (referenciaDuplicada)
                throw new InvalidOperationException("Ya existe un instrumento con esa referencia.");

            var instrumento = new Instrumento
            {
                Id = id,
                ControladorId = instrumentoActual.ControladorId,
                TipoInstrumentoId = dto.TipoInstrumentoId,
                Referencia = dto.Referencia,
                Estado = dto.Estado,
                Activo = dto.Activo
            };

            Instrumento actualizado =
                await _instrumentoRepositorio.ActualizarInstrumentoAsync(instrumento);

            return new InstrumentoRespuestaDto
            {
                Id = actualizado.Id,
                ControladorId = actualizado.ControladorId,
                TipoInstrumentoId = actualizado.TipoInstrumentoId,
                Referencia = actualizado.Referencia,
                Estado = EstadoInstrumentoMapperDto.ToDto(actualizado.Estado),
                Activo = actualizado.Activo
            };
        }

        public async Task<bool> EliminarInstrumentoAsync(long id)
        {
            bool existe =
                await _instrumentoRepositorio.ExisteInstrumentoAsync(id);

            if (!existe)
                throw new KeyNotFoundException("El instrumento no existe.");

            return await _instrumentoRepositorio.EliminarInstrumentoLogicoAsync(id);
        }

        public async Task<bool> ReactivarInstrumentoAsync(long id)
        {
            bool existe =
                await _instrumentoRepositorio.ExisteInstrumentoAsync(id);

            if (!existe)
                throw new KeyNotFoundException("El instrumento no existe.");

            return await _instrumentoRepositorio.ReactivarInstrumentoLogicoAsync(id);
        }

        public async Task<InstrumentoRespuestaDto> ObtenerInstrumentoPorIdAsync(long id)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            Instrumento? instrumento =
                await _instrumentoRepositorio.ObtenerInstrumentoPorIdAsync(id, usuarioId);

            if (instrumento is null)
                throw new KeyNotFoundException("El instrumento no existe.");

            return new InstrumentoRespuestaDto
            {
                Id = instrumento.Id,
                ControladorId = instrumento.ControladorId,
                TipoInstrumentoId = instrumento.TipoInstrumentoId,
                Referencia = instrumento.Referencia,
                Estado = EstadoInstrumentoMapperDto.ToDto(instrumento.Estado),
                Activo = instrumento.Activo,
                TipoInstrumento = instrumento.TipoInstrumento is null
                    ? null
                    : new TipoInstrumentoRespuestaDto
                    {
                        Id = instrumento.TipoInstrumento.Id,
                        Nombre = instrumento.TipoInstrumento.Nombre,
                        Descripcion = instrumento.TipoInstrumento.Descripcion,
                        UnidadMedida = instrumento.TipoInstrumento.UnidadMedida,
                        Activo = instrumento.TipoInstrumento.Activo
                    }
            };
        }

        public async Task<IEnumerable<InstrumentoRespuestaDto>> ObtenerInstrumentoPorControladorAsync(long invernaderoId)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            Invernadero? invernadero =await _invernaderoRepositorio.ObtenerInvernaderoPorIdAsync(invernaderoId,usuarioId);

            if (invernadero is null)
                throw new InvalidOperationException("El usuario no tiene un controlador IoT registrado.");

            ControladorIot? controlador = (await _controladorRepositorio
                .ObtenerControladorIotPorInvernaderoAsync(invernadero.Id)).FirstOrDefault();


            if (controlador is null)
                return Enumerable.Empty<InstrumentoRespuestaDto>();

            IEnumerable<Instrumento> instrumentos =
                await _instrumentoRepositorio.ObtenerInstrumentoPorControladorAsync(
                    controlador.Id,
                    usuarioId);

            return instrumentos.Select(instrumento => new InstrumentoRespuestaDto
            {
                Id = instrumento.Id,
                ControladorId = instrumento.ControladorId,
                TipoInstrumentoId = instrumento.TipoInstrumentoId,
                Referencia = instrumento.Referencia,
                Estado = EstadoInstrumentoMapperDto.ToDto(instrumento.Estado),
                Activo = instrumento.Activo,
                createdAt = instrumento.CreatedAt,
                updatedAt = instrumento.UpdatedAt,
                TipoInstrumento = instrumento.TipoInstrumento is null
                    ? null
                    : new TipoInstrumentoRespuestaDto
                    {
                        Id = instrumento.TipoInstrumento.Id,
                        Nombre = instrumento.TipoInstrumento.Nombre,
                        Descripcion = instrumento.TipoInstrumento.Descripcion,
                        UnidadMedida = instrumento.TipoInstrumento.UnidadMedida,
                        Activo = instrumento.TipoInstrumento.Activo
                    }
            });
        }
    }
}