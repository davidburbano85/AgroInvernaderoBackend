using NpgsqlTypes;
namespace invernaderoInteligenteBackend.Dominio.Enums.EntidadEstadoInstrumento
{
    public enum EstadoInstrumento
    {
        [PgName("BUENO")]
        BUENO,

        [PgName("MALO")]
        MALO,

        [PgName("MANTENIMIENTO")]
        MANTENIMIENTO
    }
}
