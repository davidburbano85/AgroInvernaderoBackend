using invernaderoInteligenteBackend.Aplicacion.DTO.TipoInstrumentoDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using invernaderoInteligenteBackend.Dominio.Entidades.TipoInstrumentos;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.ServiciosDto
{
    public class TipoInstrumentoServicio : ITipoInstrumentoServicio
    {
        private readonly ITipoInstrumentoRepositorio _tipoInstrumentoRepositorio;

        public TipoInstrumentoServicio(
            ITipoInstrumentoRepositorio tipoInstrumentoRepositorio)
        {
            _tipoInstrumentoRepositorio = tipoInstrumentoRepositorio;
        }

        public async Task<TipoInstrumentoRespuestaDto> CrearTipoInstrumentoAsync(
            CrearTipoInstrumentoDto dto)
        {
            bool existeNombre =await _tipoInstrumentoRepositorio.ExisteNombreTipoInstrumentoAsync(dto.Nombre);

            if (existeNombre)
                throw new InvalidOperationException("Ya existe un tipo de instrumento con ese nombre.");

            var tipoInstrumento = new TipoInstrumento
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                UnidadMedida = dto.UnidadMedida,
                Activo = true
            };

            short id =await _tipoInstrumentoRepositorio.CrearTipoInstrumentoAsync(tipoInstrumento);

            tipoInstrumento.Id = id;

            return new TipoInstrumentoRespuestaDto
            {
                Id = tipoInstrumento.Id,
                Nombre = tipoInstrumento.Nombre,
                Descripcion = tipoInstrumento.Descripcion,
                UnidadMedida = tipoInstrumento.UnidadMedida,
                Activo = tipoInstrumento.Activo
            };
        }

        public async Task<TipoInstrumentoRespuestaDto> ActualizarTipoInstrumentoAsync(
            short id,ActualizarTipoInstrumentoDto dto)

        {
            bool existe =await _tipoInstrumentoRepositorio.ExisteTipoInstrumentoAsync(id);

            if (!existe)
                throw new KeyNotFoundException("El tipo de instrumento no existe.");

            bool nombreDuplicado =await _tipoInstrumentoRepositorio.ExisteNombreActualizarTipoInstrumentoAsync(id, dto.Nombre);

            if (nombreDuplicado)
                throw new InvalidOperationException("Ya existe un tipo de instrumento con ese nombre.");

            var tipoInstrumento = new TipoInstrumento
            {
                Id = id,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                UnidadMedida = dto.UnidadMedida,
                Activo = dto.Activo
            };

            TipoInstrumento actualizado =await _tipoInstrumentoRepositorio.ActualizarTipoInstrumentoAsync(tipoInstrumento);

            return new TipoInstrumentoRespuestaDto
            {
                Id = actualizado.Id,
                Nombre = actualizado.Nombre,
                Descripcion = actualizado.Descripcion,
                UnidadMedida = actualizado.UnidadMedida,
                Activo = actualizado.Activo
            };
        }

        public async Task<TipoInstrumentoRespuestaDto> ObtenerTipoInstrumentoPorIdAsync(
            short id)
        {
            TipoInstrumento? tipoInstrumento =await _tipoInstrumentoRepositorio.ObtenerTipoInstrumentoPorIdAsync(id);

            if (tipoInstrumento is null)
                throw new KeyNotFoundException("El tipo de instrumento no existe.");

            return new TipoInstrumentoRespuestaDto
            {
                Id = tipoInstrumento.Id,
                Nombre = tipoInstrumento.Nombre,
                Descripcion = tipoInstrumento.Descripcion,
                UnidadMedida = tipoInstrumento.UnidadMedida,
                Activo = tipoInstrumento.Activo
            };
        }

        public async Task<IEnumerable<TipoInstrumentoRespuestaDto>> ObtenerTodosTipoInstrumentosAsync()
        {
            IEnumerable<TipoInstrumento> tipos =await _tipoInstrumentoRepositorio.ObtenerTodosTipoInstrumentosAsync();

            return tipos.Select(tipo => new TipoInstrumentoRespuestaDto
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre,
                Descripcion = tipo.Descripcion,
                UnidadMedida = tipo.UnidadMedida,
                Activo = tipo.Activo
            });
        }
    }
}