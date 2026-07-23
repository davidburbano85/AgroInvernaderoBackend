namespace invernaderoInteligenteBackend.Dominio.Entidades.Usuario
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Correo { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
