using SmartBook.Domain.Enums;

namespace SmartBook.Domain.Dtos.Requests.LibroRequest;

public record ConsultarLibroRequest
    (
    
    string? NombreLibro,
    string? NivelLibro,
    TipoLibro? TipoLibro,
    string Editorial,
    string Edicion
    
    
    );
