namespace invernaderoInteligenteBackend.Aplicacion.DTO.ControladorDto;

public sealed class ControladorIotRespuestaDto
{
    public long Id { get; set; }

    public long InvernaderoId { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public string? Descripcion { get; set; }
    public Guid Token { get; set; }

    public string? Ubicacion { get; set; }
    public DateTime? UltimaConexion { get; set; }

    public bool Activo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}