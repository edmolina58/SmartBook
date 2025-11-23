using SmartBook.Domain.Enums;

namespace SmartBook.Domain.Dtos.Requests.UsuarioRequest;

public record ActualizarUsuarioRequest
    (
        string? NombreUsuario,
        string? EmailUsuario,
        RolUsuario RolUsuario
    );
