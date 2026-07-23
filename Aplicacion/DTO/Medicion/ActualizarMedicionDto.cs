namespace invernaderoInteligenteBackend.Aplicacion.DTO.Medicion
{
    public class ActualizarMedicionDto
    {
       
        public string? Estado { get; set; }

        public bool Activo { get; set; }
        public DateTime FechaHora { get; set; }
    }
}