using SmartBook.Domain.Enums;

namespace SmartBook.Domain.Dtos.Requests.LibroRequest;

public record CrearLibroRequest
(
    string Nombre,
    string Nivel,
    int stock, //aqui?
    TipoLibro TipoLibro,
    string Editorial,
    string Edicion
    
    
    
    );
