using invernaderoInteligenteBackend.Dominio.Enums.EntidadEstadoInstrumento;


using invernaderoInteligenteBackend.Dominio.Entidades.TipoInstrumentos;

namespace invernaderoInteligenteBackend.Dominio.Entidades.Instrumentos
{
    public class Instrumento
    {
        public long Id { get; set; }

        public long ControladorId { get; set; }

        public short TipoInstrumentoId { get; set; }

        public string Referencia { get; set; }

        public EstadoInstrumento Estado { get; set; }

        public bool Activo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public TipoInstrumento? TipoInstrumento { get; set; }
    }
}