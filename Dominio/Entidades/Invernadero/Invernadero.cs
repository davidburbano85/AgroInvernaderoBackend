namespace invernaderoInteligenteBackend.Dominio.Entidades.Invernadero
{
    public class Invernadero
    {
        public long Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public bool Activo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
