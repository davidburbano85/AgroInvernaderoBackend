using invernaderoInteligenteBackend.Dominio.Enums.EntidadEstadoInstrumento;

namespace invernaderoInteligenteBackend.Aplicacion.DTO.InstrumentoDto
{
    public class ActualizarInstrumentoDto
    {
        public short TipoInstrumentoId { get; set; }

        public string Referencia { get; set; }

        public EstadoInstrumento Estado { get; set; }

        public bool Activo { get; set; }
    }
}