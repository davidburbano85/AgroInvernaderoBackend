using invernaderoInteligenteBackend.Aplicacion.EnumsDto.EstadoInstrumentoDto;
using invernaderoInteligenteBackend.Dominio.Enums.EntidadEstadoInstrumento;

namespace invernaderoInteligenteBackend.Infraestructura.Mapper.MapperDominio
{
    public static class EstadoInstrumentoMapper
    {
        public static EstadoInstrumento ToDominio(EstadoInstrumentoDto estadoDto)
        {
            return estadoDto switch
            {
                EstadoInstrumentoDto.BUENO => EstadoInstrumento.BUENO,
                EstadoInstrumentoDto.MALO => EstadoInstrumento.MALO,
                EstadoInstrumentoDto.MANTENIMIENTO => EstadoInstrumento.MANTENIMIENTO,
                _ => throw new ArgumentOutOfRangeException(nameof(estadoDto))
            };
        }
    }
}