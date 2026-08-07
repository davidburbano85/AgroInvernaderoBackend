
namespace invernaderoInteligenteBackend.Aplicacion.DTO.ControladorDto
{
    public class CrearControladorIotDto
    {
        public long InvernaderoId { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public DateTime UltimaConexion { get; set; }
    }
}
