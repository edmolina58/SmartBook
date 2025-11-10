
using SmartBook.Domain.Dtos.Reponses.LoginsReponse;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponse;
using SmartBook.Domain.Dtos.Requests.LoginRequest;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories;


namespace SmartBook.WebApi.Services;
public class UsuarioService 
{
    private readonly UsuarioRepository _usuarioRepository;

    public UsuarioService(UsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }
    public UsuarioReponse? Crear(CrearUsuarioRequest request)
    {
        // Validar que no exista usuario con mismo email


        // Validar que el email sea institucional
        if (!request.EmailUsuario.EndsWith("@cecar.edu.co"))
        {
            throw new BusinessRoleException("Solo se permiten correos institucionales @cecar.edu.co");
        }

        var usuario = new Usuario(
            request.IdUsuario,
            request.PasswordUsuario,
            request.NombreUsuario,
            request.EmailUsuario,
            request.RolUsuario
        );

        _usuarioRepository.Crear(usuario);

        return new UsuarioReponse(
            usuario.IdUsuario,
            usuario.Identificacion,
            usuario.Nombre,
            usuario.Email,
            usuario.RolUsuario,
            usuario.EstadoUsuario,
            usuario.FechaCreacion,
            usuario.FechaActualizacion
        );
    }

    public bool Borrar(string id)
    {
        return _usuarioRepository.Borrar(id);
    }

    public UsuarioReponse Consultar(string id)
    {
        var usuario = _usuarioRepository.Consultar(id);

        if (usuario is null)
        {
            throw new BusinessRoleException("Usuario no encontrado");
        }

        return new UsuarioReponse(
            usuario.IdUsuario,
            usuario.Identificacion,
            usuario.Nombre,
            usuario.Email,
            usuario.RolUsuario,
            usuario.EstadoUsuario,
            usuario.FechaCreacion,
            usuario.FechaActualizacion
        );
    }
    /*
    public bool Actualizar(string id, ActualizarUsuarioRequest request)
    {
        var usuario = _usuarioRepository.Consultar(id);

        if (usuario is null)
        {
            return false;
        }

        // Validar email institucional si se cambia
        if (!request.EmailUsuario.EndsWith("@cecar.edu.co"))
        {
            throw new BusinessRoleException("Solo se permiten correos institucionales @cecar.edu.co");
        }



        return _usuarioRepository.(id, request);
    }*/



    public IEnumerable<UsuarioReponse> Consultar(ConsultarUsuarioRequest request)
    {
        // Para este metodo necesitarías agregar un método en el repository
        // que consulte por nombre y rol
        var usuarios = new List<Usuario>();

        // Por ahora retornamos una lista vacía
        return usuarios.Select(MapToResponse);
    }

    public LoginReponse? Login(LoginRequest request)
    {
        var usuario = _usuarioRepository.Consultar(request.Email);

        if (usuario is null || usuario.EstadoUsuario != EstadoUsuario.Activo)
        {
            throw new BusinessRoleException("Credenciales inválidas o usuario inactivo");
        }

        // Validar contraseña
        if (usuario.Password != request.PassWord)
        {
            throw new BusinessRoleException("Credenciales inválidas");
        }

        // Generar token JWT (simulado)
        var token = GenerarTokenJWT(usuario);

        var usuarioResponse = new UsuarioReponse(
            usuario.IdUsuario,
            usuario.Identificacion,
            usuario.Nombre,
            usuario.Email,
            usuario.RolUsuario,
            usuario.EstadoUsuario,
            usuario.FechaCreacion,
            usuario.FechaActualizacion
        );

        return new LoginReponse(token, DateTime.UtcNow.AddHours(1), usuarioResponse);
    }

    public bool Activar(string id)
    {
        var usuario = _usuarioRepository.Consultar(id);

        if (usuario is null)
        {
            return false;
        }

        if (usuario.EstadoUsuario == EstadoUsuario.Activo)
        {
            throw new BusinessRoleException("El usuario ya está activo");
        }

    

        return _usuarioRepository.Borrar(id);
    }

    private string GenerarTokenJWT(Usuario usuario)
    {
        // Simulación de generación de token JWT
        return $"jwt_token_{usuario.IdUsuario}_{DateTime.UtcNow.Ticks}";
    }

    private UsuarioReponse MapToResponse(Usuario usuario)
    {
        return new UsuarioReponse(
            usuario.IdUsuario,
            usuario.Identificacion,
            usuario.Nombre,
            usuario.Email,
            usuario.RolUsuario,
            usuario.EstadoUsuario,
            usuario.FechaCreacion,
            usuario.FechaActualizacion
        );
    }
}