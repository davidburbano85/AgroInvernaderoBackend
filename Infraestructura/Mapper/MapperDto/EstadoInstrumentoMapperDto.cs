using invernaderoInteligenteBackend.Aplicacion.EnumsDto.EstadoInstrumentoDto;
using invernaderoInteligenteBackend.Dominio.Enums.EntidadEstadoInstrumento;

namespace invernaderoInteligenteBackend.Infraestructura.Mapper.MapperDto
{
    public static class EstadoInstrumentoMapperDto
    {
        public static EstadoInstrumentoDto ToDto(EstadoInstrumento estado)
        {
            return estado switch
            {
                EstadoInstrumento.BUENO => EstadoInstrumentoDto.BUENO,
                EstadoInstrumento.MALO => EstadoInstrumentoDto.MALO,
                EstadoInstrumento.MANTENIMIENTO => EstadoInstrumentoDto.MANTENIMIENTO,
                _ => throw new ArgumentOutOfRangeException(nameof(estado))
            };
        }
    }
}