using SmartBook.Domain.Enums;

namespace SmartBook.Domain.Dtos.Requests.LibroRequest;

public record CrearLibroRequest
(
    string Nombre,
    string Nivel,
    int stock, 
    TipoLibro TipoLibro,
    string Editorial,
    string Edicion
    );
