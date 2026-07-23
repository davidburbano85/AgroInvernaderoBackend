namespace invernaderoInteligenteBackend.Dominio.Entidades.TipoInstrumentos
{
    public class TipoInstrumento
    {
        public short Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UnidadMedida { get; set; }
        public bool Activo { get; set; }
    }
}
