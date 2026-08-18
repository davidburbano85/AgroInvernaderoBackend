using invernaderoInteligenteBackend.Aplicacion.DTO.InvernaderoDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using invernaderoInteligenteBackend.Dominio.Entidades.Invernadero;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.ServiciosDto
{


    public class InvernaderoServicio : IInvernaderoServicio
    {
        private readonly IInvernaderoRepositorio _invernaderoRepositorio;
        private readonly IUsuarioContext _usuarioContext;



        public InvernaderoServicio(
            IInvernaderoRepositorio invernaderoRepositorio,
            IUsuarioContext usuarioContext)
        {
            _invernaderoRepositorio = invernaderoRepositorio;
            _usuarioContext = usuarioContext;
        }

        public async Task<InvernaderoRespuestaDto> ActualizarAsync(long id, ActualizarInvernaderoDto dto)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();
            Invernadero? invernadero = await _invernaderoRepositorio
                .ObtenerInvernaderoPorIdAsync(id, usuarioId);
            if(invernadero is null)
                throw new Exception("Invernadero no encontrado o no pertenece al usuario autenticado.");

            bool existeNombre = await _invernaderoRepositorio
                .ExisteNombreActualizarInvernaderoAsync(id, usuarioId, dto.Nombre);
            if(existeNombre)
                throw new Exception("Ya existe un invernadero con ese nombre para este usuario.");


            invernadero.Nombre = dto.Nombre;
            invernadero.Ubicacion = dto.Ubicacion;

            Invernadero actualizado= await _invernaderoRepositorio.ActualizarAsync(invernadero);

            return new InvernaderoRespuestaDto
            {
                Id=actualizado.Id,
                Nombre= actualizado.Nombre,
                Ubicacion= actualizado.Ubicacion,
                Activo= actualizado.Activo,
                CreatedAt= actualizado.CreatedAt,
                UpdatedAt= actualizado.UpdatedAt,
            };

        }

        public async Task<InvernaderoRespuestaDto> CrearAsync(CrearInvernaderoDto dto)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();
            bool existeNombre= await  _invernaderoRepositorio
                .ExisteNombreInvernaderoAsync(usuarioId, dto.Nombre);
            if(existeNombre)
            {
                throw new Exception("Ya existe un invernadero con ese nombre para este usuario.");
            };

            var invernadero= new Invernadero
                {
                UsuarioId= usuarioId,
                Nombre= dto.Nombre,
                Ubicacion= dto.Ubicacion,
            };


            long id= await _invernaderoRepositorio.CrearAsync(invernadero);
            Invernadero? resultado =await _invernaderoRepositorio.ObtenerInvernaderoPorIdAsync(id, usuarioId);

            return new InvernaderoRespuestaDto {
            Id=resultado!.Id,
            Nombre= resultado.Nombre,
            Ubicacion = resultado.Ubicacion,
            Activo=resultado.Activo,
            CreatedAt= resultado.CreatedAt,
            UpdatedAt= resultado.UpdatedAt,

            };

        }

        public async Task<bool> EliminarAsync(long id)
        {
           Guid usuarioId= _usuarioContext.ObtenerAuthUserId();
            Invernadero? invernadero= await _invernaderoRepositorio
                .ObtenerInvernaderoPorIdAsync(id,usuarioId);
            if(invernadero is null)
                throw new Exception("Invernadero no encontrado o no pertenece al usuario autenticado.");

            return await _invernaderoRepositorio.EliminarLogicoAsync(id);
        }

        public async Task<bool> ReactivarAsync(long id)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();
            Invernadero? invernadero = await _invernaderoRepositorio
                .ObtenerInvernaderoPorIdAsync(id, usuarioId);
            if (invernadero is null)
                throw new Exception("Invernadero no encontrado o no pertenece al usuario autenticado.");

            return await _invernaderoRepositorio.ReactivarLogicoAsync(id);
        }

        public async Task<InvernaderoRespuestaDto?> ObtenerPorIdAsync(long id)
        {
            Guid usuarioId= _usuarioContext.ObtenerAuthUserId();
            Invernadero? invernadero= await _invernaderoRepositorio.ObtenerInvernaderoPorIdAsync(id,usuarioId);

            if(invernadero is null)
                throw new Exception("Invernadero no encontrado o no pertenece al usuario autenticado.");

            return new InvernaderoRespuestaDto
            {
                Id=invernadero.Id,
                Nombre= invernadero.Nombre,
                Ubicacion= invernadero.Ubicacion,
                Activo= invernadero.Activo,
                CreatedAt = invernadero.CreatedAt,
                UpdatedAt = invernadero.UpdatedAt,
            };
        }

        public async Task<IEnumerable<InvernaderoRespuestaDto>> ObtenerPorUsuarioAsync()
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            IEnumerable<Invernadero?> invernaderos =
                await _invernaderoRepositorio.ObtenerPorUsuarioAsync(usuarioId);

            if (invernaderos is null)
                return null;

            return  invernaderos
            .Where(invernadero=> invernadero is not null)
            .Select(i => new InvernaderoRespuestaDto
            {
                Id = i!.Id,
                Nombre = i.Nombre,
                Ubicacion = i.Ubicacion,
                Activo = i.Activo,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }
   
    
    }
}