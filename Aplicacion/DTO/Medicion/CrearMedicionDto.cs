namespace invernaderoInteligenteBackend.Aplicacion.DTO.Medicion
{
    public class CrearMedicionDto
    {
        public long InstrumentoId { get; set; }
        public decimal CoordenadaX { get; set; }
        public decimal CoordenadaY { get; set; }
        public decimal Cantidad { get; set; }
        public string? Estado { get; set; }
    }
}
