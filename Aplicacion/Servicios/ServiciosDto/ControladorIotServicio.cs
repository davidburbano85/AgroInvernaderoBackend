using invernaderoInteligenteBackend.Aplicacion.DTO.ControladorDto;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using invernaderoInteligenteBackend.Dominio.Entidades.ControladorIot;
using invernaderoInteligenteBackend.Dominio.Entidades.Invernadero;

namespace invernaderoInteligenteBackend.Aplicacion.Servicios.ServiciosDto
{
    public class ControladorIotServicio : IControladorIotServicio
    {
        private readonly IInvernaderoRepositorio _invernaderoRepositorio;
        private readonly IControladorIotRepositorio _controladorIotRepositorio;
        private readonly IUsuarioContext _usuarioContext;
        private readonly IControladorContext _controladorContext;

        public ControladorIotServicio(
            IControladorIotRepositorio controladorIotRepositorio,
            IUsuarioContext usuarioContext,
            IInvernaderoRepositorio invernaderoRepositorio,
            IControladorContext controladorContext)
        {
            _controladorIotRepositorio = controladorIotRepositorio;
            _usuarioContext = usuarioContext;
            _invernaderoRepositorio = invernaderoRepositorio;
            _controladorContext = controladorContext;
        }


        public async Task<ControladorIotRespuestaDto> CrearControladorIotAsync(CrearControladorIotDto dto)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

           IEnumerable <Invernadero?> invernaderos =
                await _invernaderoRepositorio.ObtenerPorUsuarioAsync(usuarioId);

            Invernadero? invernadero = null;
            foreach (var inv in invernaderos)
            {
                if (inv is not null  && inv.Id==dto.InvernaderoId)
                {
                    invernadero = inv;
                    break;
                }
            }

            if(invernadero is null)
                throw new InvalidOperationException("El invernadero no existe o no pertenece al usuario autenticado.");

            bool existeControlador =
                await _controladorIotRepositorio.ExisteControladorPorInvernaderoAsync(invernadero.Id);


            if (existeControlador)
                throw new InvalidOperationException("El invernadero ya tiene un controlador IoT.");


            var controlador = new ControladorIot
            {
                InvernaderoId = invernadero.Id,
                Nombre = dto.Nombre,
                Token = Guid.NewGuid(),
                Ubicacion = dto.Ubicacion,
                Referencia = dto.Referencia,
                Descripcion = dto.Descripcion,
                Activo = true,
                CreatedAt=DateTime.UtcNow,
                UpdatedAt=DateTime.UtcNow,
                UltimaConexion=dto.UltimaConexion
            };

            long  id = await _controladorIotRepositorio.CrearControladorIotAsync(controlador);

            controlador.Id = id;


            return new ControladorIotRespuestaDto
            {
                Id = controlador.Id,
                InvernaderoId = controlador.InvernaderoId,
                Nombre = controlador.Nombre,
                Token= controlador.Token,
                Ubicacion = controlador.Ubicacion,
                Referencia = controlador.Referencia,
                Descripcion = controlador.Descripcion,
                UltimaConexion = controlador.UltimaConexion,
                UpdatedAt=controlador.UpdatedAt,
                CreatedAt=controlador.CreatedAt,
                Activo = controlador.Activo
            };
        }


        public async Task<ControladorIotRespuestaDto> ActualizarControladorIotAsync(long id,ActualizarControladorIotDto dto)
        {
            var controlador = new ControladorIot
            {
                Id = id,
                Nombre = dto.Nombre,
                Ubicacion = dto.Ubicacion,
                Referencia = dto.Referencia,
                Descripcion = dto.Descripcion,
                UltimaConexion = dto.UltimaConexion,
                UpdatedAt=DateTime.UtcNow
                

            }; 


            ControladorIot actualizado =
                await _controladorIotRepositorio.ActualizarControladorIotAsync(controlador);


            return new ControladorIotRespuestaDto
            {
                Id = actualizado.Id,
                InvernaderoId = actualizado.InvernaderoId,
                Nombre = actualizado.Nombre,
                Ubicacion = actualizado.Ubicacion,
                Referencia = actualizado.Referencia,
                Descripcion = actualizado.Descripcion,
                UltimaConexion = actualizado.UltimaConexion,
                Activo = actualizado.Activo,
                UpdatedAt=actualizado.UpdatedAt
            };
        }


        public async Task<bool> EliminarControladorIotAsync(long id)
        {
            bool existe =
                await _controladorIotRepositorio.ExisteControladorIotAsync(id);


            if (!existe)
                throw new KeyNotFoundException("El controlador IoT no existe.");


            return await _controladorIotRepositorio
                .EliminarLogicoControladorIotAsync(id);
        }


      


        public async Task<ControladorIotRespuestaDto?> ObtenerControladorIotPorInvernaderoAsync(long invernaderoId)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();


           IEnumerable< Invernadero?> invernaderos =
                await _invernaderoRepositorio.ObtenerPorUsuarioAsync(usuarioId);


          Invernadero? invernadero = invernaderos.FirstOrDefault(x=> x?.Id == invernaderoId);
            if(invernadero is null)
                throw new InvalidOperationException("El invernadero no existe o no pertenece al usuario autenticado.");

            var controladores =
                await _controladorIotRepositorio
                    .ObtenerControladorIotPorInvernaderoAsync(invernadero.Id);


            ControladorIot? controlador =
                controladores.FirstOrDefault();


            if (controlador is null)
                return null;


            return new ControladorIotRespuestaDto
            {
                Id = controlador.Id,
                InvernaderoId = controlador.InvernaderoId,
                Nombre = controlador.Nombre,
                Ubicacion = controlador.Ubicacion,
                Referencia = controlador.Referencia,
                Descripcion = controlador.Descripcion,
                UltimaConexion = controlador.UltimaConexion,
                Activo = controlador.Activo,
                Token=controlador.Token
            };
        }


        public async Task<bool> ReactivarControladorIotAsync(long id)
        {
            bool existe =
                await _controladorIotRepositorio.ExisteControladorIotAsync(id);


            if (!existe)
                throw new KeyNotFoundException("El controlador IoT no existe.");


            return await _controladorIotRepositorio
                .ReactivarLogicoControladorIotAsync(id);
        }
    }
}