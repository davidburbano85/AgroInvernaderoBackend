namespace invernaderoInteligenteBackend.Aplicacion.DTO.InstrumentoDto;

using invernaderoInteligenteBackend.Aplicacion.DTO.TipoInstrumentoDto;
using invernaderoInteligenteBackend.Aplicacion.EnumsDto.EstadoInstrumentoDto;

public sealed class InstrumentoRespuestaDto
{
    public long Id { get; set; }

    public long ControladorId { get; set; }

    public short TipoInstrumentoId { get; set; }

    public string Referencia { get; set; } = string.Empty;
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }

    public EstadoInstrumentoDto Estado { get; set; }

    public bool Activo { get; set; }

    public TipoInstrumentoRespuestaDto? TipoInstrumento { get; set; }
}