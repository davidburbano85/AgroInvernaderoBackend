namespace invernaderoInteligenteBackend.Aplicacion.DTO.Medicion
{
    public class MedicionRespuestaDto
    {
        public long Id { get; set; }

        public long InstrumentoId { get; set; }

        public decimal CoordenadaX { get; set; }

        public decimal CoordenadaY { get; set; }

        public decimal Cantidad { get; set; }

        public DateTime FechaHora { get; set; }

        public string? Estado { get; set; }

        public bool Activo { get; set; }
    }
}