namespace invernaderoInteligenteBackend.Aplicacion.DTO.InvernaderoDto
{
    public class InvernaderoRespuestaDto
    {
        public long Id { get; set; }

        public string Nombre { get; set; }

        public string Ubicacion { get; set; }

        public bool Activo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}