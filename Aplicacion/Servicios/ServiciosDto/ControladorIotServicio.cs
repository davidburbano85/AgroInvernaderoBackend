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

        public ControladorIotServicio(
            IControladorIotRepositorio controladorIotRepositorio,
            IUsuarioContext usuarioContext,
            IInvernaderoRepositorio invernaderoRepositorio)
        {
            _controladorIotRepositorio = controladorIotRepositorio;
            _usuarioContext = usuarioContext;
            _invernaderoRepositorio = invernaderoRepositorio;
        }


        public async Task<ControladorIotRespuestaDto> CrearControladorIotAsync(CrearControladorIotDto dto)
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();

            Invernadero? invernadero =
                await _invernaderoRepositorio.ObtenerPorUsuarioAsync(usuarioId);

            if (invernadero is null)
                throw new InvalidOperationException("El usuario no tiene un invernadero registrado.");


            bool existeControlador =
                await _controladorIotRepositorio.ExisteControladorPorInvernaderoAsync(invernadero.Id);


            if (existeControlador)
                throw new InvalidOperationException("El invernadero ya tiene un controlador IoT.");


            var controlador = new ControladorIot
            {
                InvernaderoId = invernadero.Id,
                Nombre = dto.Nombre,
                Ubicacion = dto.Ubicacion,
                Referencia = dto.Referencia,
                Descripcion = dto.Descripcion,
                Activo = true
            };


            long id =
                await _controladorIotRepositorio.CrearControladorIotAsync(controlador);


            controlador.Id = id;


            return new ControladorIotRespuestaDto
            {
                Id = controlador.Id,
                InvernaderoId = controlador.InvernaderoId,
                Nombre = controlador.Nombre,
                Ubicacion = controlador.Ubicacion,
                Referencia = controlador.Referencia,
                Descripcion = controlador.Descripcion,
                UltimaConexion = controlador.UltimaConexion,
                Activo = controlador.Activo
            };
        }


        public async Task<ControladorIotRespuestaDto> ActualizarControladorIotAsync(
            long id,
            ActualizarControladorIotDto dto)
        {
            var controlador = new ControladorIot
            {
                Id = id,
                Nombre = dto.Nombre,
                Ubicacion = dto.Ubicacion,
                Referencia = dto.Referencia,
                Descripcion = dto.Descripcion,
                UltimaConexion = dto.UltimaConexion
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
                Activo = actualizado.Activo
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


      


        public async Task<ControladorIotRespuestaDto?> ObtenerControladorIotPorInvernaderoAsync()
        {
            Guid usuarioId = _usuarioContext.ObtenerAuthUserId();


            Invernadero? invernadero =
                await _invernaderoRepositorio.ObtenerPorUsuarioAsync(usuarioId);


            if (invernadero is null)
                return null;


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
                Activo = controlador.Activo
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