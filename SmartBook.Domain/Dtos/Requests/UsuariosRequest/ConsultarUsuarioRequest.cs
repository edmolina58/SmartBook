using SmartBook.Domain.Enums;

namespace SmartBook.Domain.Dtos.Requests.UsuarioRequest;

public record ConsultarUsuarioRequest
(
    // es necesario consultarlo asi?
    // es mejor no es mejor ID?
    string? IdUsuario,
    string? NombreUsuario,
    RolUsuario? RolUsuario
    
    
    );
