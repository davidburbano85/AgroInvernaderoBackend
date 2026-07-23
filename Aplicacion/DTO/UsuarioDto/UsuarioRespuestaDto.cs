namespace invernaderoInteligenteBackend.Aplicacion.DTO.UsuarioDto
{
    public class UsuarioRespuestaDto
    {
        public Guid Id { get; set; }

        public string Correo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}