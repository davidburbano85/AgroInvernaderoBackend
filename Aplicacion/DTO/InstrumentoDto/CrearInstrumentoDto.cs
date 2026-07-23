using invernaderoInteligenteBackend.Aplicacion.EnumsDto.EstadoInstrumentoDto;

namespace invernaderoInteligenteBackend.Aplicacion.DTO.InstrumentoDto
{
    public class CrearInstrumentoDto
    {
        public short TipoInstrumentoId { get; set; }
        public EstadoInstrumentoDto Estado { get; set; }
        public string Referencia { get; set; }
    }
}
