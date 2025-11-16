using SmartBook.Domain.Enums;

namespace SmartBook.Domain.Dtos.Requests.UsuarioRequest;

public record CrearUsuarioRequest
(
    string? identificacion,
    string? PasswordUsuario,
    string? NombreUsuario,
    string? EmailUsuario,
    RolUsuario RolUsuario
    );
