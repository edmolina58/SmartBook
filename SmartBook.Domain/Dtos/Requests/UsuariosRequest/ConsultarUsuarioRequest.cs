using SmartBook.Domain.Enums;

namespace SmartBook.Domain.Dtos.Requests.UsuarioRequest;

public record ConsultarUsuarioRequest
(

    string? NombreUsuario,
    RolUsuario? RolUsuario
    
    
    );
