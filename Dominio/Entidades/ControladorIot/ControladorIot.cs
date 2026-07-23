namespace invernaderoInteligenteBackend.Dominio.Entidades.ControladorIot
{
    public class ControladorIot
    {
        public long Id { get; set; }
        public long InvernaderoId { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public DateTime? UltimaConexion { get; set; }
        public bool Activo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
