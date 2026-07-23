namespace invernaderoInteligenteBackend.Aplicacion.DTO.TipoInstrumentoDto;

public sealed class TipoInstrumentoRespuestaDto
{
    public long Id { get; set; }

    public string Nombre { get; set; } 
    public string UnidadMedida { get; set; } 


    public string? Descripcion { get; set; }

    public bool Activo { get; set; }
}