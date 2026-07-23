using invernaderoInteligenteBackend.Dominio.Entidades.Instrumentos;

namespace invernaderoInteligenteBackend.Dominio.Entidades.Mediciones
{
    public class Medicion
    {
        public long Id { get; set; }

        public long InstrumentoId { get; set; }

        public decimal CoordenadaX { get; set; }

        public decimal CoordenadaY { get; set; }

        public decimal Cantidad { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.Now;

        public string? Estado { get; set; }

        public bool Activo { get; set; }

        public Instrumento? Instrumento { get; set; }
    }
}