namespace invernaderoInteligenteBackend.Aplicacion.DTO.ControladorDto
{
    public class ActualizarControladorIotDto
    {
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public DateTime UltimaConexion { get; set; }
        public string Referencia { get; set; }

        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
