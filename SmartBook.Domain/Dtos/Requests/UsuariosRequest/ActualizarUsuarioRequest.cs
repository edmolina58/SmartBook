using SmartBook.Domain.Enums;

namespace SmartBook.Domain.Dtos.Requests.UsuarioRequest;

public record ActualizarUsuarioRequest
    (
    string? NombresUsuario,
    string? EmailUsuario,
    RolUsuario RolUsuario,
    EstadoUsuario EstadoUsuario   
    );
